/*
* AVR_Ventivez_Mega8L.c
*
* Created: 2015.05.22. 21:09:04
*  Author: János
*/

#define F_CPU 7372800

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <compat/twi.h>
#include <math.h>

#define SCL_CLOCK 10000
#define TWIprescaler_value 1
#define TWIBaudRate ((F_CPU/SCL_CLOCK)-16)/(2*TWIprescaler_value)
volatile char Mega48_TWIcim = 0xA0;
volatile char TWISlaveCim;
#define TWI_IR 0
#define TWI_OLVAS 1

volatile short TWIM48ValaszSzamlalo = 31111;
#define TWIM48ValaszSzamlaloMaxido 290 //ms
#define TWIM48ValaszSzamlaloMaxertek (((double)TWIM48ValaszSzamlaloMaxido*(double)F_CPU)/((double)262144000)) //262144000 = 256*1024*1000 (1024-es előosztás)

char i2CHibaM48RESETSzamlalo = 0;
char i2CHibaTELJESRESETSzamlalo = 0;
#define i2CHibaM48RESETSzamlaloMaxertek 4
#define i2CHibaTELJESRESETSzamlaloMaxertek 10

///////TWI bufferek/////
volatile char TWIkuldendo[20];
volatile char TWIkuldendoHanyadik = 0, TWIkuldendoHossz = 0;
volatile char TWIvisszaolvasson = 0;

volatile char TWIbejovo[20];
volatile char TWIbejovoHossz = 0, TWIbejovoHanyadik = 0, TWIujbejovo = 0;
volatile char TWIkuldjvissza = 0;

volatile char twiRepstartolt = 0;
////////////////

#define USART_BAUDRATE 9600  // soros kommunikacio sebessege: 9600 bps
#define UBRR_ERTEK ((F_CPU / (USART_BAUDRATE * 16UL)) - 1)  // UBRR

#define UARTKod100 0 //HOZZÁADVA A KOMMUNIKÁCIÓKEZDŐ KÓDOKHOZ. 0, VAGY 100. 7 BITES, ÉS 8 BITES KÓDOLÁS. 8 BITESHEZ UARTKod100 = 100, 7 BITESHEZ UARTKod100 = 0

#define SZENZARANY (double)2.55

volatile char kittenyek[8];
volatile char UJkittenyek[8];
volatile char AlapKittenyek[8];
volatile char hanyadikKit = 0;
volatile char UARTStatus = 0; //0: Uresjarat 1: Aktualis 2: Alapertelmezett 3:Biztonsági szenzor
volatile char WDKorok = 0;
volatile short NemKapJelet = 0;
volatile char KittenyKesz = 0;
volatile short SZENZ1 = 0, SZENZ2 = 0;
volatile char FinomFordszValtasKell = 0;
volatile char AlapFordszIrKell = 0;
volatile char BiztSzenzorIrKell = 0;


volatile char BiztHofEllenorzSzamlalo = 0, BiztBuzzerSzamlalo = 0, BuzzerKell = 0, BuzzerSzamlMAX = 100;
volatile uint16_t BiztHoFesz1 = 255, BiztHoFesz2 = 255;
volatile uint16_t BiztHoFesz1FOGADBUFF = 255, BiztHoFesz2FOGADBUFF = 255;
unsigned char i2c_write( unsigned char data )
{
	uint8_t   twst;
	//  adat irasa a korabban megcimzett eszkozbe
	TWDR = data;
	TWCR = (1<<TWINT) | (1<<TWEN);
	// varakozas amig az atvitel be nem fejezodik
	while(!(TWCR & (1<<TWINT)));
	// TWI Statusz Register ertekenek ellenorzese, elooszto bitek kimaszkolasa.
	twst = TW_STATUS & 0xF8;
	if( twst != TW_MT_DATA_ACK) return 1;
	return 0;
}
unsigned char EEPROM_read( unsigned int uiAddress)
{
	/* Wait for completion of previous write */
	while(EECR & (1<<EEWE))
	;
	/* Set up address register */
	EEAR = uiAddress;
	/*
	Start eeprom read by writing EERE */
	EECR |= (1<<EERE);
	/* Return data from data register */
	return EEDR;
}
unsigned char EEPROMOlvBizt( unsigned int uiAddress)
{
	unsigned char a = 0;
	unsigned char b = 1;
	while(a != b)
	{
		a = EEPROM_read(uiAddress);
		b = EEPROM_read(uiAddress);
	}
	return a;
}
void EEPROM_write(unsigned int uiAddress, unsigned char ucData)
{
	//Wait for completion of previous write
	while(EECR & (1<<EEWE));

	/* Set up address and data registers */
	EEAR = uiAddress;
	EEDR = ucData;
	/*
	Write logical one to EEMWE */
	cli();
	EECR |= (1<<EEMWE);
	/* Start eeprom write by setting EEWE */
	EECR |= (1<<EEWE);
	sei();
}
void EEPROMIrBizt(unsigned int uiAddress, unsigned char ucData)
{
	while(EEPROM_read(uiAddress) != ucData)
	{
		EEPROM_write(uiAddress, ucData);
	}
}
void i2c_stop(void)
{
	// STOP jel kuldese
	TWCR = (1<<TWINT) | (1<<TWEN) | (1<<TWSTO);
	//  varakozas amig a Stop utasitas vegre nem hajtodik es az I2C busz szabad nem lesz
	while(TWCR & (1<<TWSTO));
}
void i2c_start_wait(unsigned char address)
{
	uint8_t   twst;
	while ( 1 )
	{
		// START jel kuldese
		TWCR = (1<<TWINT) | (1<<TWSTA) | (1<<TWEN);
		// varakozas amig az atvitel be nem fejezodik
		while(!(TWCR & (1<<TWINT)));
		// TWI Statusz Register ertekenek ellenorzese, elooszto bitek kimaszkolasa.
		twst = TW_STATUS & 0xF8;
		if ( (twst != TW_START) && (twst != TW_REP_START)) continue;
		// eszkozazonosito cim kuldese
		TWDR = address;
		TWCR = (1<<TWINT) | (1<<TWEN);
		// varakozas amig az atvitel be nem fejezodik
		while(!(TWCR & (1<<TWINT)));
		// TWI Statusz Register ertekenek ellenorzese, elooszto bitek kimaszkolasa.
		twst = TW_STATUS & 0xF8;
		if ( (twst == TW_MT_SLA_NACK )||(twst ==TW_MR_DATA_NACK) )
		{
			// az eszkoz foglalt, Stop jel kuldese, irasi muvelet befejezese
			TWCR = (1<<TWINT) | (1<<TWEN) | (1<<TWSTO);
			//  varakozas amig a Stop utasitas vegre nem hajtodik es az I2C busz szabad nem lesz
			while(TWCR & (1<<TWSTO));
			continue;
		}
		//if( twst != TW_MT_SLA_ACK) return 1;
		break;
	}
}

void Konfig10bitADC()    // ADC konfiguralas (beallitas)
{
	ADMUX |= (1<<REFS0);    // Vcc mint referencia
	ADCSRA = (1<<ADEN) | (1<<ADPS1) | (1<<ADPS0);  // ADC engedelyezese, ADC eloosztas = 8 (125 KHz)
}

unsigned int Beolvas10bitADC(unsigned char csatorna)
{
	ADMUX = (ADMUX & 0b11110000) | csatorna;    //ADC csatorna kivalasztasa
	ADCSRA |= (1<<ADSC);    // ADC konverzio elinditasa
	while (ADCSRA & (1<<ADSC));    // varas az atalakitasra
	ADCSRA |= (1<<ADSC);         // konverzió elindítás
	while (ADCSRA & (1<<ADSC));    // varas az atalakitasra
	return (ADCL | (ADCH<<8));    // ADC ertek kiolvasasa
}

void KonfigUART()  // UART beallitasa
{
	// 9600 bps soros kommunikacio sebesseg beallitasa
	UBRRL = UBRR_ERTEK;        // UBRR_ERTEK also 8 bitjenek betoltese az UBRRL regiszterbe
	UBRRH = (UBRR_ERTEK>>8);   // UBRR_ERTEK felso 8 bitjenek betoltese az UBRRH regiszterbe
	// Aszinkron mod, 8 Adat Bit, Nincs Paritas Bit, 1 Stop Bit
	UCSRC |= (1 << URSEL) | (1 << UCSZ0) | (1 << UCSZ1);
	//Ado es Vevo aramkorok bekapcsolasa + az RX interrupt engedelyezese
	UCSRB |= (1 << RXEN) | (1 << RXCIE) | (1 << TXEN);   //
}

int main(void)
{
	//_delay_ms(50);
	init();

	//for (int i = 0; i<2; ++i)///DEBUG INDULÁS BEEP
	//{
	//PORTD |= (1<<PORTD3);
	//_delay_ms(50);
	//PORTD &= ~(1<<PORTD3);
	//_delay_ms(50);
	//}

	while(1)
	{
		WDKorok = 0;
		if(KittenyKesz == 1)
		{
			KittenyBeallito();
			KittenyKesz = 0;
		}
		
		WDKorok = 0;
		if(FinomFordszValtasKell)
		{
			char vanelteres = 0;
			int i;
			for (i=0;i<8;++i)
			{
				if(kittenyek[i] != UJkittenyek[i])
				{
					if(kittenyek[i] > UJkittenyek[i])
					{
						if(kittenyek[i] - UJkittenyek[i] >= 2 )
						{kittenyek[i] -= 2;}
						else
						{kittenyek[i] = UJkittenyek[i];}
					}
					else
					{
						if(UJkittenyek[i] - kittenyek[i] >= 1 )
						{kittenyek[i] += 1;}
						else
						{kittenyek[i] = UJkittenyek[i];}
					}

					vanelteres = 1;
				}
			}
			if(vanelteres)
			{
				KittenyKesz = 1;
			}
			FinomFordszValtasKell = 0;
		}
		
		WDKorok = 0;
		if(AlapFordszIrKell)
		{
			char i;
			for (i=0; i < 8; ++i)
			{
				if(AlapKittenyek[i] <2)
				EEPROMIrBizt(i,0);
				else
				EEPROMIrBizt(i,AlapKittenyek[i]);
			}
			AlapFordszIrKell = 0;
		}
		
		WDKorok = 0;
		if(BiztSzenzorIrKell)
		{
			EEPROMIrBizt(24, BiztHoFesz1 & 0b00000000011111);
			EEPROMIrBizt(25,(BiztHoFesz1 & 0b00001111100000)>>5);
			EEPROMIrBizt(26, BiztHoFesz2 & 0b00000000011111);
			EEPROMIrBizt(27,(BiztHoFesz2 & 0b00001111100000)>>5);
			BiztSzenzorIrKell = 0;
		}
		
		WDKorok = 0;
		if(BiztHofEllenorzSzamlalo > 65)
		{
			BiztHofEllenorzSzamlalo = 0;

			short beolvasott;

			beolvasott = Beolvas10bitADC(0);
			if(beolvasott < 904)
			SZENZ1=beolvasott;//23.89795-22.75286*log((((double)5/(((double)5/(double)256)*beolvasott))-1)-0.00851);
			else
			SZENZ1 = 0;

			beolvasott = Beolvas10bitADC(1);
			if(beolvasott < 904)
			SZENZ2=beolvasott;//23.89795-22.75286*log((((double)5/(((double)5/(double)256)*beolvasott))-1)-0.00851);
			else
			SZENZ2 = 0;
			
			
			if(BiztHoFesz1 <= SZENZ1 || BiztHoFesz2 <= SZENZ2)
			{
				KittenyKesz = 1;
				BuzzerKell = 1;
			}
			else
			{
				BuzzerKell = 0;
			}
		}
		
		WDKorok = 0;
		if(TWIM48ValaszSzamlalo != 31111 && TWIM48ValaszSzamlalo >= TWIM48ValaszSzamlaloMaxertek)
		{
			KittenyKesz = 1;
			
			++i2CHibaM48RESETSzamlalo;
			++i2CHibaTELJESRESETSzamlalo;
		}
		
		WDKorok = 0;
		if(TWIujbejovo)
		{
			if(TWIbejovo[0] > 1)
			{
				char behossz = TWIbejovoHossz;
				char beuzenet[behossz];
				int i = 0;
				for(; i < behossz; ++i)
				{
					beuzenet[i] = TWIbejovo[i];
				}
				switch(beuzenet[1])
				{
					case 17://Helyes CRC
					{
						i2CHibaM48RESETSzamlalo = 0;
						i2CHibaTELJESRESETSzamlalo = 0;

						TWIM48ValaszSzamlalo = 31111;
						break;
					}
					default://Helytelen CRC
					{
						++i2CHibaM48RESETSzamlalo;
						++i2CHibaTELJESRESETSzamlalo;

						KittenyKesz = 1;
						break;
					}
				}
			}
			TWIujbejovo = 0;
		}
		
		if(i2CHibaM48RESETSzamlalo >= i2CHibaM48RESETSzamlaloMaxertek)
		{
			RESET_M48();
			i2CHibaM48RESETSzamlalo = 0;
		}
		if(i2CHibaTELJESRESETSzamlalo >= i2CHibaTELJESRESETSzamlaloMaxertek)
		{
			RESET();
		}
	}
}
void init()
{
	DDRB |= (1<<PINB5);

	Konfig10bitADC();
	
	TCCR0 |= (1<<CS02) | (1<<CS00);//F_CPU/1024
	TIMSK |= (1<<TOIE0);
	PORTC |= (1<<PINC3);
	DDRC |= (1<<PINC3);
	DDRD |= (1<<PIND2) |(1<<PIND3);
	
	KonfigUART();
	
	TWBR = TWIBaudRate;
	TWCR = (1<<TWEN) | (1<<TWINT) | (1<<TWIE);
	
	//TCCR1A |= (1<<COM1A1) | (1<<COM1B1) | (1<<WGM10);
	TCCR1B |= (1<<WGM12) | (1<<CS10);
	DDRB |= (1<<PINB1) | (1<<PINB2);
	
	BiztHoFesz1 = BiztHoFesz2 = 0;
	BiztHoFesz1 = EEPROMOlvBizt(24);
	BiztHoFesz1 |= EEPROMOlvBizt(25)<<5;
	BiztHoFesz2 = EEPROMOlvBizt(26);
	BiztHoFesz2 |= EEPROMOlvBizt(27)<<5;

	char i;
	for (i=0; i < 8; ++i)
	{
		kittenyek[i] = 0;
		UJkittenyek[i] = EEPROMOlvBizt(i);
	}
	
	WDKorok = 0;

	//_delay_ms(5000);
	//RESET_MCP2200();
	sei();
	//ADCSRA |= (1<<ADEN) | (1<<ADFR) | (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2) | (1<<ADSC);
	//char csatorna = 0;
	//ADMUX = 0b00000000 | (1<<ADLAR) | csatorna;
}
unsigned short const crc16table[256] = {
	0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
	0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
	0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
	0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
	0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
	0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
	0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
	0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
	0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
	0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
	0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
	0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
	0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
	0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
	0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
	0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
	0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
	0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
	0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
	0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
	0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
	0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
	0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
	0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
	0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
	0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
	0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
	0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
	0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
	0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
	0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
	0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
};
unsigned short Crc16Szamolo(char *adat, char hossz)
{
	unsigned short crc16 = 0;

	int i;
	for (i=0; i<hossz; ++i)
	{
		crc16 = (crc16 >> 8) ^ crc16table[(crc16 ^ adat[i]) & 0xff];//saját táblás
	}

	return crc16;
}
char M48nakKuld()
{
	char kd[10];
	kd[0] = TWIkuldendoHossz = 10;
	kd[1] = 20;//Fordszámküldés kódja
	WDKorok = 0;

	if(kittenyek[1] < 2)
	{
		kd[2] = 0;
	}
	else kd[2] = kittenyek[1];
	if(kittenyek[2] < 2)
	{
		kd[3] = 0;
	}
	else kd[3] = kittenyek[2];
	if(kittenyek[3] < 2)
	{
		kd[4] = 0;
	}
	else kd[4] = kittenyek[3];
	if(kittenyek[4] < 2)
	{
		kd[5] = 0;
	}
	else kd[5] = kittenyek[4];
	if(kittenyek[5] < 2)
	{
		kd[6] = 0;
	}
	else kd[6] = kittenyek[5];
	if(kittenyek[6] < 2)
	{
		kd[7] = 0;
	}
	else kd[7] = kittenyek[6];
	
	unsigned short crc16 = Crc16Szamolo(kd, 8);
	kd[8] = crc16 & 0x00FF;
	kd[9] = (crc16 & 0xFF00)>>8;

	int i = 0;
	for(;i < TWIkuldendoHossz;++i)
	{
		TWIkuldendo[i] = kd[i];
	}
	
	WDKorok = 0;

	twi_Kuld_kuldendo(Mega48_TWIcim,1);
	TWIM48ValaszSzamlalo = 0;

	return 0;
}
void KittenyBeallito()
{
	char i;
	if(BiztHoFesz1 <= SZENZ1 || BiztHoFesz2 <= SZENZ2)
	{
		for (i=0; i < 8; ++i)
		{
			kittenyek[i] = 255;
		}
	}

	WDKorok = 0;

	if(kittenyek[0] < 2)
	{OCR1A = 0; TCCR1A &= ~(1<<COM1A1);}
	else
	{OCR1A = kittenyek[0]; TCCR1A |= (1<<COM1A1);}
	
	if(kittenyek[7] < 2)
	{OCR1B = 0; TCCR1A &= ~(1<<COM1B1);}
	else
	{OCR1B = kittenyek[7]; TCCR1A |= (1<<COM1B1);}
	
	WDKorok = 0;

	M48nakKuld();//////////////DEBUGHOZ KIKAPCSOLVA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
}
void RESET()
{
	RESET_M48();
	WDTCR |= (1<<WDE);
}
void RESET_M48()
{
	PORTC &= ~(1<<PINC3);
	_delay_ms(10);
	PORTC |= (1<<PINC3);
}
ISR(TIMER0_OVF_vect)
{
	FinomFordszValtasKell = 1;

	if(TWIM48ValaszSzamlalo <= TWIM48ValaszSzamlaloMaxertek)
	++TWIM48ValaszSzamlalo;

	++BiztBuzzerSzamlalo;
	++BiztHofEllenorzSzamlalo;
	++WDKorok;
	++NemKapJelet;
	if(WDKorok > 120)
	RESET();
	
	if(NemKapJelet > 800)
	{
		RESET();;//////////////DEBUGHOZ KIKAPCSOLVA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	}

	if(BiztBuzzerSzamlalo > BuzzerSzamlMAX)
	{
		BiztBuzzerSzamlalo = 0;

		if(BuzzerKell)
		{
			if(PIND & (1<<PIND3))
			PORTD &= ~(1<<PORTD3);
			else
			PORTD |= (1<<PORTD3);

			switch (BuzzerSzamlMAX)
			{
				case 20:
				BuzzerSzamlMAX = 2;
				break;
				case 2:
				BuzzerSzamlMAX = 4;
				break;
				case 4:
				BuzzerSzamlMAX = 5;
				break;
				case 5:
				BuzzerSzamlMAX = 8;
				case 8:
				BuzzerSzamlMAX = 10;
				break;
				case 10:
				BuzzerSzamlMAX = 12;
				break;
				case 12:
				BuzzerSzamlMAX = 6;
				break;
				case 6:
				BuzzerSzamlMAX = 3;
				break;
				case 3:
				BuzzerSzamlMAX = 20;
				break;
				default:
				BuzzerSzamlMAX = 2;
				break;
			}
		}
		else
		{
			PORTD &= ~(1<<PORTD3);
			BuzzerSzamlMAX = 100;
		}
	}
}
void UARTAdatKuld(char adat)  // Ez a fuggveny a kuldendo adatot beirja az UDR regiszter kimeno pufferjebe
{
	while(!(UCSRA & (1<<UDRE))) // Varakozas amig az Ado kesz nem lesz az adatkuldesre
	{
		//   Varakozas
	}
	// Az Ado mar kesz az adatkuldesre, a kuldendo adatot a kimeno pufferjebe irjuk
	UDR=adat;
}

ISR(USART_RXC_vect)
{
	if(UDR == 255)//KÉZFOGÁS
	{
		NemKapJelet = 0;
		UARTStatus = 0;
		
		UARTAdatKuld(125);
		_delay_ms(100);
		UARTAdatKuld(150);
		_delay_ms(100);
		UARTAdatKuld(10);
		_delay_ms(10);
		UARTAdatKuld(20);
		_delay_ms(10);
		UARTAdatKuld(30);
		_delay_ms(10);
		UARTAdatKuld(40);
		_delay_ms(10);
		UARTAdatKuld(50);
		_delay_ms(10);
		UARTAdatKuld(60);
		_delay_ms(10);
		UARTAdatKuld(70);
		_delay_ms(10);
		UARTAdatKuld(80);
		_delay_ms(10);
		UARTAdatKuld(35);
		_delay_ms(10);
		UARTAdatKuld(36);
		_delay_ms(10);
		NemKapJelet = 0;
	}
	else if(UDR == 107 + UARTKod100)//AKTUÁLIS FORDSZ
	{
		NemKapJelet = 0;
		
		UARTStatus = 1;
		hanyadikKit = 0;
		UARTAdatKuld(107 + UARTKod100);
	}
	else if(UDR == 125 + UARTKod100)//ALAPÉRTELMEZETT FORDSZ
	{
		NemKapJelet = 0;
		
		UARTStatus = 2;
		hanyadikKit = 0;
		UARTAdatKuld(125 + UARTKod100);
	}
	else if(UDR == 110 + UARTKod100)//ALAPÉRT VISSZA
	{
		NemKapJelet = 0;
		UARTStatus = 0;

		UARTAdatKuld(110 + UARTKod100);

		char VisszKittenyek[8];
		char i;
		for (i=0; i < 8; ++i)
		{
			VisszKittenyek[i] = (char)(round((double)EEPROM_read(i)/(double)2.55));
		}

		char CRC1 = 0, CRC2 = 0, CRC3 = 0;

		for (i=0; i < 8; ++i)
		{

			if(VisszKittenyek[i] & 0b00000001)
			{++CRC1;++CRC3;}
			if(VisszKittenyek[i] & 0b00000010)
			{++CRC1; ++CRC2;}
			if(VisszKittenyek[i] & 0b00000100)
			{++CRC1;++CRC3;}
			if(VisszKittenyek[i] & 0b00001000)
			{++CRC1;++CRC2;}
			if(VisszKittenyek[i] & 0b00010000)
			{++CRC1;++CRC3;}
			if(VisszKittenyek[i] & 0b00100000)
			{++CRC1;++CRC2;}
			if(VisszKittenyek[i] & 0b01000000)
			{++CRC1;++CRC3;}
			if(VisszKittenyek[i] & 0b10000000)
			{++CRC1;++CRC2;}
			
		}

		for (i=0; i < 8; ++i)
		{
			UARTAdatKuld(VisszKittenyek[i]);
		}
		UARTAdatKuld(CRC1);
		UARTAdatKuld(CRC2);
		UARTAdatKuld(CRC3);

	}
	else if(UDR == 115 + UARTKod100) //Hőmérő Érték Kérés
	{
		NemKapJelet = 0;
		UARTStatus = 0;
		UARTAdatKuld(115 + UARTKod100);
		
		short beolvasott;

		beolvasott = Beolvas10bitADC(0);

		
		if(beolvasott > 120)
		SZENZ1=beolvasott;//23.89795-22.75286*log((((double)5/(((double)5/(double)256)*beolvasott))-1)-0.00851);
		else
		SZENZ1 = 0;

		beolvasott = Beolvas10bitADC(1);
		if(beolvasott > 120)
		SZENZ2=beolvasott;//23.89795-22.75286*log((((double)5/(((double)5/(double)256)*beolvasott))-1)-0.00851);
		else
		SZENZ2 = 0;

		char KULD1L = (SZENZ1 & 0b0000000000111111),KULD1H = ((SZENZ1 & 0b0000111111000000)>>6), KULD2L = (SZENZ2 & 0b0000000000111111), KULD2H = ((SZENZ2 & 0b0000111111000000)>>6);
		UARTAdatKuld(KULD1L);
		UARTAdatKuld(KULD1H);
		UARTAdatKuld(KULD2L);
		UARTAdatKuld(KULD2H);

		char CRC1 = 0, CRC2 = 0, CRC3 = 0;

		if(KULD1L & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD1L & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD1L & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD1L & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD1H & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD1H & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD1H & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD1H & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD2L & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD2L & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD2L & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD2L & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD2H & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD2H & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD2H & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD2H & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b10000000)
		{++CRC1;++CRC2;}

		
		UARTAdatKuld(CRC1);
		UARTAdatKuld(CRC2);
		UARTAdatKuld(CRC3);
		
	}
	else if(UDR == 118 + UARTKod100) //Biztonsági Érték Kérés
	{
		NemKapJelet = 0;
		UARTStatus = 0;
		UARTAdatKuld(118 + UARTKod100);
		
		//char KULD1 = BiztHoFesz1/SZENZARANY, KULD2 = BiztHoFesz2/SZENZARANY;
		char KULD1L = (BiztHoFesz1 & 0b0000000000011111),KULD1H = ((BiztHoFesz1 & 0b0000001111100000)>>5), KULD2L = (BiztHoFesz2 & 0b0000000000011111), KULD2H = ((BiztHoFesz2 & 0b0000001111100000)>>5);
		UARTAdatKuld(KULD1L);
		UARTAdatKuld(KULD1H);
		UARTAdatKuld(KULD2L);
		UARTAdatKuld(KULD2H);

		char CRC1 = 0, CRC2 = 0, CRC3 = 0;

		if(KULD1L & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD1L & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD1L & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD1L & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD1L & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD1H & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD1H & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD1H & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD1H & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD1H & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD2L & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD2L & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD2L & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD2L & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD2L & 0b10000000)
		{++CRC1;++CRC2;}

		if(KULD2H & 0b00000001)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00000010)
		{++CRC1; ++CRC2;}
		if(KULD2H & 0b00000100)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00001000)
		{++CRC1;++CRC2;}
		if(KULD2H & 0b00010000)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b00100000)
		{++CRC1;++CRC2;}
		if(KULD2H & 0b01000000)
		{++CRC1;++CRC3;}
		if(KULD2H & 0b10000000)
		{++CRC1;++CRC2;}

		
		UARTAdatKuld(CRC1);
		UARTAdatKuld(CRC2);
		UARTAdatKuld(CRC3);
		
	}
	else if(UDR == 120 + UARTKod100)//Biztonsági Szenzorok Megadása
	{
		NemKapJelet = 0;

		UARTAdatKuld(120 + UARTKod100);
		UARTStatus = 3;

		BiztHoFesz1FOGADBUFF = BiztHoFesz2FOGADBUFF = 0;

		hanyadikKit = 0;

	}
	else if(UDR < 101  + UARTKod100)
	{
		if(UARTStatus == 1)
		{
			NemKapJelet = 0;
			
			UJkittenyek[hanyadikKit] = (char)(round((double)UDR*(double)2.55));
			++ hanyadikKit;
			if(hanyadikKit == 8)
			{
				UARTAdatKuld(220);
				hanyadikKit = UARTStatus = 0;

				//if(PORTB & (1<<PINB5))
				//	PORTB &= ~(1<<PINB5);
				//else
					//PORTB |= (1<<PINB5);
			}
			
			NemKapJelet = 0;
			UARTAdatKuld(UDR);
		}
		else if(UARTStatus == 2)
		{
			NemKapJelet = 0;
			
			AlapKittenyek[hanyadikKit] = (char)(round((double)UDR*(double)2.55));
			++ hanyadikKit;
			if(hanyadikKit == 8)
			{
				UARTAdatKuld(220);
				hanyadikKit = UARTStatus = 0;
				
				AlapFordszIrKell = 1;
			}
			
			NemKapJelet = 0;
			UARTAdatKuld(UDR);
		}
		else if(UARTStatus == 3)
		{
			NemKapJelet = 0;

			if(hanyadikKit == 0)
			{BiztHoFesz1FOGADBUFF = UDR; ++hanyadikKit;}
			else if(hanyadikKit == 1)
			{BiztHoFesz1FOGADBUFF |= (UDR<<6); ++hanyadikKit;}
			else if(hanyadikKit == 2)
			{BiztHoFesz2FOGADBUFF = UDR; ++hanyadikKit;}
			else if(hanyadikKit == 3)
			{
				UARTAdatKuld(220);
				BiztHoFesz2FOGADBUFF |= (UDR<<6);
				BiztHoFesz1 = BiztHoFesz1FOGADBUFF;
				BiztHoFesz2 = BiztHoFesz2FOGADBUFF;
				
				BiztSzenzorIrKell = 1;

				hanyadikKit = UARTStatus = 0;
			}
			UARTAdatKuld(UDR);
		}
	}
}

#pragma region TWI kezeles
void twi_master_start(char repstart)
{
	twiRepstartolt = repstart;
	// Enable interrupt and start bits
	TWCR = (1 << TWINT)| (1 << TWEN) | (1 << TWSTA) | (1<<TWIE);
}
//void twi_master_Repstart()
//{
//twiRepstartolt = 1;
//// Enable interrupt and start bits
//TWCR = (1 << TWINT)| (1 << TWEN) | (1 << TWSTA) | (1<<TWIE);
//}
void twi_master_stop()
{
	// Set stop bit
	TWCR = (1 << TWINT) | (1 << TWEN) | (1 << TWSTO)| (1<<TWIE);
}

void twi_setAck()
{
	TWCR = (1 << TWINT) | (1 << TWEN) | (1 << TWEA)| (1<<TWIE);
}

void twi_setNack()
{
	TWCR = (1 << TWINT) | (1 << TWEN)| (1<<TWIE);
}

void twi_Kuld_kuldendo(char SlaveCim, char VisszOlv)
{
	TWISlaveCim = SlaveCim;
	TWIvisszaolvasson = VisszOlv;
	TWIkuldendoHossz = TWIkuldendo[0];
	TWIkuldendoHanyadik = 0;
	twi_master_stop();
	while(TWCR & (1<<TWINT));
	twi_master_start(0);
}
ISR(TWI_vect)
{
	//UARTAdatKuld(TWSR & 0b11111000); UARTAdatKuld(TWSR & 0b11111000); UARTAdatKuld(TWSR & 0b11111000);
	//UARTAdatKuld(TWDR);
	switch (TWSR & 0b11111000)
	{
		//////////////////////MASTER TRANSMITTER///////////////////////
		case 0x08://A START condition transmitted
		case 0x10://A repeated START condition transmitted
		{
			if(twiRepstartolt == 0)
			{
				TWDR = TWISlaveCim + TWI_IR;
				twi_setAck();
			}
			else
			{
				if(TWIvisszaolvasson)
				{
					TWDR = TWISlaveCim + TWI_OLVAS;
					twi_setAck();
				}
			}
			break;
		}
		case 0x18://SLA+W transmitted; ACK received
		{
			if(TWIkuldendoHanyadik >= TWIkuldendoHossz)
			{
				if(TWIvisszaolvasson)
				{
					twi_master_start(0);
				}
				else
				{
					twi_master_stop();
				}
			}
			else
			{
				TWDR = TWIkuldendo[TWIkuldendoHanyadik];
				++TWIkuldendoHanyadik;
				twi_setAck();
			}
			break;
		}
		case 0x30://Data byte transmitted; NOT ACK received
		//{
		//twi_master_stop();
		//break;
		//}
		case 0x20://SLA+W transmitted; NOT ACK received
		{
			twi_master_stop();
			break;
		}
		case 0x28://Data byte transmitted; ACK received
		{
			if(TWIkuldendoHanyadik < TWIkuldendoHossz)
			{
				TWDR = TWIkuldendo[TWIkuldendoHanyadik];
			}
			++TWIkuldendoHanyadik;

			if(TWIkuldendoHanyadik > TWIkuldendoHossz)
			{
				if(TWIvisszaolvasson)
				{
					twi_master_start(1);
				}
				else
				{
					twi_master_stop();
				}
			}
			break;
		}
		//case 0x38://Arbitration lost in SLA+W or data bytes
		//{//Kikommentezve, mert nem fér a FLASH-be
		//
		//break;
		//}
		//////////////////////MASTER RECEIVER///////////////////////
		case 0x40://SLA+R transmitted; ACK received
		{
			TWIbejovoHanyadik = 0;
			TWIujbejovo = 0;
			twi_setAck();
			break;
		}
		//case 0x48://SLA+R transmitted; NOT ACK received
		//{//Kikommentezve, mert nem fér a FLASH-be
		//break;
		//}
		case 0x50://Data byte received; ACK returned
		{
			if(TWIbejovoHanyadik == 0)
			{
				if(TWDR != 255)
				TWIbejovoHossz = TWDR;
				else
				TWIbejovoHossz = 1;
			}

			if(TWIbejovoHanyadik < TWIbejovoHossz)
			{
				TWIbejovo[TWIbejovoHanyadik] = TWDR;
				twi_setAck();
			}
			
			++TWIbejovoHanyadik;
			
			if(TWIbejovoHanyadik >= TWIbejovoHossz)
			{
				TWIujbejovo = 1;
				twi_master_stop();
				//UARTStringKuld("STOPPED\r\n",9);
			}
			break;
		}
		//case 0x58://Data byte received; NOT ACK returned
		//{//Kikommentezve, mert nem fér a FLASH-be
		//
		//break;
		//}
	}
	//Kikommentezve, mert nem fér a FLASH-be
	//////////////////////SLAVE RECEIVER///////////////////////
	//switch (TWSR & 0b11111000)
	//{
	//case 0x60://Own SLA+W  received; ACK  returned
	//{
	//bejovoHanyadik = 0;
	//ujbejovo = 0;
	//twi_setAck();
	//break;
	//}
	//case 0x68://Arbitration lost in SLA+R/W as Master; own SLA+W  received; ACK returned
	//{
	//
	//break;
	//}
	//case 0x70://General call address received; ACK returned
	//{
	//
	//break;
	//}
	//case 0x78://Arbitration lost in SLA+R/W as Master; General call address received; ACK returned
	//{
	//
	//break;
	//}
	//case 0x80://Previously addressed with own SLA+W; data received; ACK  returned
	//{
	//if(bejovoHanyadik == 0)
	//{
	//bejovoHossz = TWDR;
	//}
	//
	//if(bejovoHanyadik < bejovoHossz)
	//{
	//bejovo[bejovoHanyadik] = TWDR;
	////twi_setAck();
	//}
	//
	//++bejovoHanyadik;
	//
	//if(bejovoHanyadik >= bejovoHossz)
	//{
	//ujbejovo = 1;
	//}
	//break;
	//}
	//case 0x88://Previously addressed with own SLA+W; data received; NOT ACK  returned
	//{
	//
	//break;
	//}
	//case 0x90://Previously addressed with general call; data received; ACK  returned
	//{
	//
	//break;
	//}
	//case 0x98://Previously addressed with general call; data received; NOT ACK returned
	//{
	//
	//break;
	//}
	//case 0xA0://A STOP condition or repeated START condition received while still addressed as Slave
	//{
	//
	//break;
	//}
	////////////////////////SLAVE TRANSMITTER///////////////////////
	//case 0xA8://Own SLA+R received; ACK returned
	//{
	//if(bejovoHossz > 1)
	//{
	//switch (bejovo[1])
	//{
	//case 65:
	//{
	//kuldjvissza = 1;
	//kuldendoHossz = 3;
	//kuldendo[0] = 3;
	//kuldendo[1] = 70;
	//kuldendo[2] = ADCH;
	//
	//kuldendoHanyadik = 1;
	//TWDR = kuldendo[0];
	//}
	//break;
	//}
	//}
	//break;
	//}
	//case 0xB0://Arbitration lost in SLA+R/W as Master; own SLA+R received; ACK returned
	//{
	//
	//break;
	//}
	//case 0xB8://Data byte in TWDR transmitted; ACK received
	//{
	//if(kuldjvissza)
	//{
	//if(kuldendoHanyadik < kuldendoHossz)
	//{
	//TWDR = kuldendo[kuldendoHanyadik];
	//
	//if(kuldendoHanyadik == kuldendoHossz -1)
	//{
	//twi_setNack();
	//}
	//++kuldendoHanyadik;
	//}
	//else
	//{
	//TWDR=233;
	//}
	//}
	//else
	//{
	//TWDR=0;
	//}
	//break;
	//}
	//case 0xC0://Data byte in TWDR transmitted; NOT ACK received
	//{
	//
	//break;
	//}
	//case 0xC8://Last data byte in TWDR transmitted (TWEA = “0”); ACK received
	//{
	//twi_setAck();
	//break;
	//}
	//}

	TWCR |= 0x80;// Clear interrupt flag bit
}

#pragma endregion TWI kezeles