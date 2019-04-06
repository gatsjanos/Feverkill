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

#define SCL_CLOCK 10000
#define TWIprescaler_value 1
#define TWIBaudRate ((F_CPU/SCL_CLOCK)-16)/(2*TWIprescaler_value)
#define Mega48_TWIcim  0xA0
#define TWI_IR 0
#define TWI_OLVAS 1

#define USART_BAUDRATE 9600  // soros kommunikacio sebessege: 9600 bps
#define UBRR_ERTEK ((F_CPU / (USART_BAUDRATE * 16UL)) - 1)  // UBRR


volatile char kittenyek[8];
volatile char AlapKittenyek[8];
volatile char hanyadikKit = 0;
volatile char UARTStatus = 0; //0: Uresjarat 1: Aktualis 2: Alapertelmezett
volatile char WDKorok = 0;
volatile short NemKapJelet = 0;
volatile char KittenyKesz = 0;
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
void EEPROM_write( unsigned int uiAddress, char Data)
{
	/* 
	Wait for completion of previous write
	 */
	while(EECR & (1<<EEWE));
	/* Set up address and data registers */
	EEAR = uiAddress;
	EEDR = Data;
	/* 
	Write logical one to EEMWE */
	cli();
	EECR |= (1<<EEMWE);
	/* Start eeprom write by setting EEWE */
	EECR |= (1<<EEWE);
	sei();
}
char EEPROM_read(int uiAddress)
{
	/* Wait for completion of previous write */
	while(EECR & (1<<EEWE));
	/* Set up address register */
	EEAR = uiAddress;
	/*
	Start eeprom read by writing EERE */
	EECR |= (1<<EERE);
	/* Return data from data register */
	return EEDR;
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
	_delay_ms(50);
	init();
	
    while(1)
    {
		WDKorok = 0;
		if(KittenyKesz == 1)
		{
			KittenyBeallito();
			KittenyKesz = 0;
		}
		//OCR1A = ADCH;
		
		//i2c_start_wait(Mega48_TWIcim + TWI_IR);
		//
		//if(i2c_write(ADCH) == 1)
		//continue;
		//if(i2c_write(0) == 1)
		//continue;
		//if(i2c_write(60) == 1)
		//continue;
		//if(i2c_write(2*ADCH) == 1)
		//continue;
		//if(i2c_write(ADCH) == 1)
		//continue;
		//if(i2c_write(255) == 1)
		//continue;
		//
		//i2c_stop();
		
	}
}

void init()
{
	
	WDKorok = 0;
	TCCR0 |= (1<<CS02) | (1<<CS00);
	TIMSK |= (1<<TOIE0) | (1<<TOIE1);
	PORTC |= (1<<PINC3);
	DDRC |= (1<<PINC3);
	PORTD |= (1<<PIND2);
	DDRD |= (1<<PIND2);
	
	KonfigUART();
	
	TWBR = TWIBaudRate;
	TWCR = (1<<TWEN) | (1<<TWINT) | (1<<TWIE);
	
	TCCR1A |= (1<<COM1A1) | (1<<COM1B1) | (1<<WGM10);
	TCCR1B |= (1<<WGM12) | (1<<CS10);
	DDRB |= (1<<PINB1) | (1<<PINB2);     // PORTB PB1 lab kimenet
	
	char i;
	for (i=0; i < 8; ++i)
	{
		kittenyek[i] = EEPROM_read(i);
	}
	KittenyBeallito();
	
	_delay_ms(5000);
	RESET_MCP2200();
	sei();
	//ADCSRA |= (1<<ADEN) | (1<<ADFR) | (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2) | (1<<ADSC);
	//char csatorna = 0;
	//ADMUX = 0b00000000 | (1<<ADLAR) | csatorna;
}
void KittenyBeallito()
{
	//if(kittenyek[0] < 2)
	//{OCR1A = 0; TCCR1A &= ~(1<<COM1A1);} //////////28KHz
	//else
	//{OCR1A = kittenyek[0]; TCCR1A |= (1<<COM1A1);} //////////28KHz
	//
	//if(kittenyek[7] < 2)
	//{OCR1B = 0; TCCR1A &= ~(1<<COM1B1);} //////////28KHz
	//else
	//{OCR1B = kittenyek[7]; TCCR1A |= (1<<COM1B1);} //////////28KHz

	
	if(kittenyek[0] < 2)
	{OCR1A = 0; TCCR1A &= ~(1<<COM1A1);}
	else
	{OCR1A = kittenyek[0] + 155; TCCR1A |= (1<<COM1A1);}
	
	if(kittenyek[7] < 2)
	{OCR1B = 0; TCCR1A &= ~(1<<COM1B1);}
	else
	{OCR1B = kittenyek[7] + 155; TCCR1A |= (1<<COM1B1);}
	
	char i;
	for (i=0; i < 5; ++i)
	{
		
		i2c_start_wait(Mega48_TWIcim + TWI_IR);
		
		if(kittenyek[1] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[1]) == 1)
		continue;
		if(kittenyek[2] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[2]) == 1)
		continue;
		if(kittenyek[3] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[3]) == 1)
		continue;
		if(kittenyek[4] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[4]) == 1)
		continue;
		if(kittenyek[5] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[5]) == 1)
		continue;
		if(kittenyek[6] < 2)
		{
			if(i2c_write(0)== 1)
			continue;
		}
		else if(i2c_write(kittenyek[6]) == 1)
		continue;
		
		i2c_stop();
		
		return 0;
	}
	
	RESET();
}

ISR(TIMER1_OVF_vect)
{
	TCNT1 = 155;
}
void RESET()
{
	RESET_M48();
	RESET_MCP2200();
	WDTCR |= (1<<WDE);
}
void RESET_M48()
{
	PORTC &= ~(1<<PINC3);
	_delay_ms(10);
	PORTC |= (1<<PINC3);
}
void RESET_MCP2200()
{
	PORTD &= ~(1<<PIND2);
	_delay_ms(300);
	PORTD |= (1<<PIND2);
}
ISR(TIMER0_OVF_vect)
{
	++WDKorok;
	++NemKapJelet;
	if(WDKorok > 120)
	RESET();
	
	if(NemKapJelet > 800)
	RESET();
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
	if(UDR == 255)
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
	else if(UDR == 107)
	{
		NemKapJelet = 0;
		
		UARTStatus = 1;
		hanyadikKit = 0;
		UARTAdatKuld(107);
	}
	else if(UDR == 125)
	{
		NemKapJelet = 0;
		
		UARTStatus = 2;
		hanyadikKit = 0;
		UARTAdatKuld(125);
	}
	else if(UDR < 101)
	{
		if(UARTStatus == 1)
		{
			NemKapJelet = 0;
			
			//kittenyek[hanyadikKit] = (char)((double)UDR*(double)2.55); //////////28KHz
			kittenyek[hanyadikKit] = UDR;
			++ hanyadikKit;
			if(hanyadikKit == 8)
			{
				UARTAdatKuld(120);
				hanyadikKit = UARTStatus = 0;
				KittenyKesz = 1;
			}
			
			NemKapJelet = 0;
			UARTAdatKuld(UDR);
		}
		else if(UARTStatus == 2)
		{
			NemKapJelet = 0;
			
			//AlapKittenyek[hanyadikKit] = (char)((double)UDR*(double)2.55); //////////28KHz
			AlapKittenyek[hanyadikKit] = UDR;
			++ hanyadikKit;
			if(hanyadikKit == 8)
			{
				UARTAdatKuld(120);
				hanyadikKit = UARTStatus = 0;
				
				char i;
				for (i=0; i < 8; ++i)
				{
					if(AlapKittenyek[i] <2)
					EEPROM_write(i,0);
					else
					EEPROM_write(i,AlapKittenyek[i]);
				}
			}
			
			NemKapJelet = 0;
			UARTAdatKuld(UDR);
		}
	}
}