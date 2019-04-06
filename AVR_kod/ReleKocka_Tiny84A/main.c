/*
* ReleKocka_Tiny84A.c
*
* Created: 2017. 03. 09. 22:42:02
* Author : gatsj
*/

//#define F_CPU 8000000
#define F_CPU 8275000//A bels� oscillator nem pontosan 8MHz, ez�rt itt korrig�lva van --> BE KELL HANGOLNI!!!!

#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>

#define ADCAtlagoltMeresekSzama 2500

char torzitasmaxSzazalek = 3;
char minimalisPWMfrekiHz = 60;
float N = 100;//null�tmenetek sz�ma m�sodpercenk�nt

volatile char masodpercSzamlalo = 0;
ISR(TIM0_OVF_vect)
{
	++masodpercSzamlalo;
	if(masodpercSzamlalo >= 127)
	{
		masodpercSzamlalo = 0;
		
	}
}

char osztoPrimek[18];//18db pr�m: maximum 61*2=122 null�tmenetsz�mig (N)
char osztoPrimekCount = 18;
void PrimOsztotombLetrehozo()
{
	char tombcount = 0;
	char prim;
	char szam = 2, i = 0;
	for (; szam * 2 <= N; ++szam)
	{
		prim = 1;
		for (i = 2; 2*i <= szam;++i)
		{
			if(szam % i == 0)
			{
				prim = 0;
				break;
			}
		}
		if(prim == 1)
		{
			osztoPrimek[tombcount] = szam;
			++tombcount;
		}
	}
	osztoPrimekCount = tombcount;
}

char szamlalo = 255, nevezo = 255, vanszintvaltas = 1;
void Egyszerusit(char szamlbe, char nevbe)
{
	vanszintvaltas = 1;
	if(szamlbe == 0)
	{
		vanszintvaltas = 0;
		nevbe = 10;
	}
	else if(szamlbe == nevbe)
	{
		vanszintvaltas = 0;
		szamlbe = nevbe = 10;
	}
	else
	{
		int i;
		for (i = 0; i < osztoPrimekCount; ++i)
		{
			while (nevbe%osztoPrimek[i] == 0 && szamlbe%osztoPrimek[i] == 0)
			{
				nevbe /= osztoPrimek[i];
				szamlbe /= osztoPrimek[i];
			}
		}
	}
	szamlalo = szamlbe;
	nevezo = nevbe;
}
char kimenetUzemmod = 1;//0: Maxim�lis torz�t�s  1: Minim�lis PWM freki  2: Ki-Be m�d
void TorzitoErtekado(float kitteny)
{
	char szamlaloVegleges = 0;
	char nevezoVegleges = 200;
	char vanszintvaltasVegleges = 1;
	if(kimenetUzemmod == 0)
	{//Maxim�lis torz�t�s
		char torzitasmaxNben = (char)((short)torzitasmaxSzazalek*N/100);
		short i;
		for (i = -torzitasmaxNben; i <= torzitasmaxNben; ++i)
		{
			if((short)(kitteny*N + i) >= 0 && (short)(kitteny*N + i) <= N)
			{
				Egyszerusit(kitteny*N + i, N);
				if(nevezo < nevezoVegleges || vanszintvaltas == 0)
				{
					nevezoVegleges = nevezo;
					szamlaloVegleges = szamlalo;
					vanszintvaltasVegleges = vanszintvaltas;

					if(vanszintvaltasVegleges == 0)
					{
						break;
					}
				}
			}
		}
	}
	else if(kimenetUzemmod == 1)
	{//Minim�lis PWM freki
		Egyszerusit(kitteny*N, N);
		nevezoVegleges = nevezo;
		szamlaloVegleges = szamlalo;
		vanszintvaltasVegleges = vanszintvaltas;

		if(vanszintvaltasVegleges &&  N/nevezoVegleges < minimalisPWMfrekiHz)
		{
			int i;
			for (i = 1; i < N/2 + 2; ++i)
			{

				if((char)(kitteny*N + i) <= N)
				{
					Egyszerusit(kitteny*N  + i, N);
					if(nevezo < nevezoVegleges || vanszintvaltas == 0)
					{
						nevezoVegleges = nevezo;
						szamlaloVegleges = szamlalo;
						vanszintvaltasVegleges = vanszintvaltas;
					}

					if(N/nevezoVegleges >= minimalisPWMfrekiHz || vanszintvaltasVegleges == 0)
					{
						break;
					}
				}
				if((short)(kitteny*N - i) >= 0 )
				{
					Egyszerusit(kitteny*N  - i, N);
					if(nevezo < nevezoVegleges || vanszintvaltas == 0)
					{
						nevezoVegleges = nevezo;
						szamlaloVegleges = szamlalo;
						vanszintvaltasVegleges = vanszintvaltas;
					}

					if(N/nevezoVegleges >= minimalisPWMfrekiHz || vanszintvaltasVegleges == 0)
					{
						break;
					}
				}
			}
		}
	}
	else
	{//Ki-Be m�d
		if(kitteny > 0.5)
		{
			Timer16_Kittenybeallito(1);
		}
		else
		{
			Timer16_Kittenybeallito(0);
		}
		return;
	}
	float kittenyVegleges = szamlaloVegleges/(float)nevezoVegleges;
	if(vanszintvaltasVegleges)
	{
		float frekvVegleges = N/(float)nevezoVegleges;
		if(frekvVegleges <= 0)
		{
			frekvVegleges = 2;
		}
		else
		{
			while(frekvVegleges < 2)//val�sz�n�leg 1
			{
				frekvVegleges *= 2;
			}
		}
		Timer16_FrekEsKittenybeallito(frekvVegleges, kittenyVegleges);
	}
	else
	{
		Timer16_Kittenybeallito(kittenyVegleges);
	}

}

void Timer16_Kittenybeallito(float kitteny)
{
	OCR1A = kitteny*ICR1;
	if(OCR1A < TCNT1 && (PORTA & (1<<PORTA6)))
	{
		PORTA &= ~(1<<PORTA6);
	}
}
void Timer16_Frekbeallito(float frekv)
{
	ICR1 = F_CPU/(64*frekv) - 1;
	if(TCNT1 > ICR1)
	{
		TCNT1 = TCNT1%ICR1;
		if(TCNT1 < OCR1A)
		{
			PORTA |= (1<<PORTA6);
		}
	}
}
void Timer16_FrekEsKittenybeallito(float frekv, float kitteny)
{
	Timer16_Frekbeallito(frekv);
	Timer16_Kittenybeallito(kitteny);
}

void SetADCCsat(char csat)
{
	ADMUX &= (0b11000000);
	ADMUX |= csat;
}
void SetADCCsatDiff()
{
	ADMUX &= (0b11000000);
	ADMUX |=  0b00010000;//Negat�v: ADC3   Pozit�v: ADC2
}
void SetADCrefAREF()
{
	ADMUX &= ~(1<<REFS0) & ~(1<<REFS1);
}
void SetADCrefVCC()
{
	ADMUX &= ~(1<<REFS1);
	ADMUX |= (1<<REFS0);
}

volatile unsigned long  int adcosszeg = 0;
volatile unsigned int adcmertdarabszam = 0;

//volatile char adcegymeresfolyamatban = 0;
//ISR(ADC_vect)
//{
//adcosszeg += ADCH;
//++adcmertdarabszam;
//adcegymeresfolyamatban = 0;
//}

void ADCEgymeresAtlagba()
{
	int i;
	for (i = 0; (ADCSRA & (1<<ADSC)) && i < 50; ++i)//max 500us v�rakoz�s
	{
		_delay_us(10);
	}

	//adcegymeresfolyamatban = 1;
	//ADCSRA &= ~(1<<ADATE);//AutoTrigger kikapcs
	//ADCSRA |= (1<<ADEN) | (1<<ADIE);//Enged�lyez + interrupt bekapcs
	//ADCSRA |= (1<<ADEN);//Enged�lyez
	ADCSRA |= (1<<ADSC);//Ind�t egy konverzi�t

	for (i = 0; (ADCSRA & (1<<ADSC)) && i < 150; ++i)//max 1500us v�rakoz�s
	{
		_delay_us(10);
	}

	adcosszeg += ADCH;
	++adcmertdarabszam;
}
char ADCEgymeresVisszater()
{
	int i;
	for (i = 0; (ADCSRA & (1<<ADSC)) && i < 50; ++i)//max 500us v�rakoz�s
	{
		_delay_us(10);
	}

	ADCSRA |= (1<<ADSC);//Ind�t egy konverzi�t

	for (i = 0; (ADCSRA & (1<<ADSC)) && i < 150; ++i)//max 1500us v�rakoz�s
	{
		_delay_us(10);
	}

	return ADCH;
}
//void StartADCAREFFreerunMeres()//Free run m�dba kapcsolja, �s a megszak�t�sban addig m�r, am�g meg nem lesz el�g sample
//{
////adcegymeresfolyamatban = 1;
//ADCSRA &= ~(1<<ADIE);//interrupt kikapcs
//ADCSRA |= (1<<ADEN) | (1<<ADATE);//Enged�lyez + autotrigger bekapcs
//ADCSRA |= (1<<ADSC);//Ind�t egy konverzi�t
////FreeRun: ADCSRB 3db LS bitje 0 �rt�k�. Alapb�l az az �rt�k�k, �gy ehhez nem kell �ket piszk�lni.
//}
//void StartADCVCCFreerunMeres()//Free run m�dba kapcsolja, �s a megszak�t�sban addig m�r, am�g meg nem lesz el�g sample
//{
////adcegymeresfolyamatban = 1;
//ADCSRA &= ~(1<<ADIE);//interrupt kikapcs
//ADCSRA |= (1<<ADEN) | (1<<ADATE);//Enged�lyez + autotrigger bekapcs
//ADCSRA |= (1<<ADSC);//Ind�t egy konverzi�t
////FreeRun: ADCSRB 3db LS bitje 0 �rt�k�. Alapb�l az az �rt�k�k, �gy ehhez nem kell �ket piszk�lni.
//}
void KikapcsADC()
{
	ADCSRA &= ~(1<<ADEN);
	//adcegymeresfolyamatban = 0;
}
void BekapcsADC()
{
	ADCSRA |= (1<<ADEN);
}

void init()
{
	//TCCR0B |= (1<<CS02);//F_CPU/256  ==> 8300000/(256*256) = 122.0703125
	//TIMSK0 |= (1<<TOIE0);


	DDRA |= (1<<PINA6);
	
	OCR1A = 0;
	TCCR1A |= (1<<WGM11);
	TCCR1B |= (1<<WGM12);
	TCCR1B |= (1<<WGM13);
	TCCR1B |= (1<<CS11) | (1<<CS10);//MINIM�LIS FREKVENCIA: 2Hz
	TCCR1A |= (1<<COM1A1);

	ADCSRA |= (1<<ADPS2) | (1<<ADPS0) | (1<<ADEN);//F_CPU/32  8MHz->250KHz
	ADCSRB |= (1<<ADLAR);
	SetADCCsat(3);

	PrimOsztotombLetrehozo();

	sei();
}

int main(void)
{
	init();
	Timer16_Kittenybeallito(0.3);
	Timer16_Frekbeallito(20);
	//StartADCFreerunMeres();
	kimenetUzemmod = 0;
	//DDRA &= ~(1<<PINA6);
	while (1)
	{
		//////////Param�terbeolvas�s/////
		SetADCrefVCC();
		SetADCCsat(1);

		int;
		for (i = 0; i < 3; i++)
		{
			ADCEgymeresVisszater();
		}	
		float buff = ADCEgymeresVisszater()/(float)255;

		if(buff < 0.016)
		{
			buff = 0;
		}
		else if(buff > 0.98)
		{
			buff = 1;
		}

		if(kimenetUzemmod == 0)
		{//Maxim�lis Torz�t�s

			torzitasmaxSzazalek = buff*50;
			if(torzitasmaxSzazalek < 2)
			{
				torzitasmaxSzazalek = 2;
			}
		}
		else if(kimenetUzemmod == 1)
		{//Minim�lis PWM
			
			minimalisPWMfrekiHz = buff*50;
			if(minimalisPWMfrekiHz < 2)
			{
				minimalisPWMfrekiHz = 2;
			}
		}
		else
		{//Ki-Be m�d
			
		}

		//////////�tlagm�r�s/////
		KikapcsADC();
		SetADCrefAREF();
		SetADCCsatDiff();
		BekapcsADC();
		int;
		for (i = 0; i < 10; i++)
		{
			ADCEgymeresVisszater();
		}
		while(1)
		{
			if(adcmertdarabszam < ADCAtlagoltMeresekSzama)
			{
				ADCEgymeresAtlagba();
			}
			else
			{
				TorzitoErtekado( ( (float)( (double)adcosszeg/(double)adcmertdarabszam ) )/(float)255);

				adcmertdarabszam = 0;
				adcosszeg = 0;
				break;
			}
		}
		//Timer16_Kittenybeallito(ADCH/(float)255);
		//TorzitoErtekado(ADCH/(float)255);
		int i;
		for (i = 0; i < 30; ++i	)
		{
			_delay_ms(10);
		}
		//PORTA |= (1<<PINA6);
		//_delay_ms(10);
		//PORTA &= ~(1<<PINA6);
	}
}

