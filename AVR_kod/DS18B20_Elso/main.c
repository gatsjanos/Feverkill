/*
* DS18B20_Elso.c
*
* Created: 2016. 06. 10. 17:11:20
* Author : gatsj
*/

#define F_CPU 7372800
#define USART_BAUDRATE 9600  // soros kommunikacio sebessege: 9600 bps
#define UBRR_ERTEK ((F_CPU / (USART_BAUDRATE * 16UL)) - 1)  // UBRR

#define LOOP_CYCLES 8//Number of cycles that the loop takes
#define us(num) (num/(LOOP_CYCLES*(1/(F_CPU/1000000.0)))) //therm_delay(us(480)); //We want to make a 480?s delay

/* Thermometer Connections (At your choice) */
#define THERM1_PORT   PORTC
#define THERM1_DDR    DDRC
#define THERM1_PIN    PINC
#define THERM1_DQ     PC0
/* Utils */
#define THERM1_INPUT_MODE()    THERM1_DDR&=~(1<<THERM1_DQ)
#define THERM1_OUTPUT_MODE()   THERM1_DDR|=(1<<THERM1_DQ)
#define THERM1_LOW()           THERM1_PORT&=~(1<<THERM1_DQ)
#define THERM1_HIGH()          THERM1_PORT|=(1<<THERM1_DQ)

/* Thermometer Connections (At your choice) */
#define THERM2_PORT   PORTC
#define THERM2_DDR    DDRC
#define THERM2_PIN    PINC
#define THERM2_DQ     PC1
/* Utils */
#define THERM2_INPUT_MODE()    THERM2_DDR&=~(1<<THERM2_DQ)
#define THERM2_OUTPUT_MODE()   THERM2_DDR|=(1<<THERM2_DQ)
#define THERM2_LOW()           THERM2_PORT&=~(1<<THERM2_DQ)
#define THERM2_HIGH()          THERM2_PORT|=(1<<THERM2_DQ)

#define THERM_CMD_CONVERTTEMP    0x44
#define THERM_CMD_RSCRATCHPAD    0xbe
#define THERM_CMD_WSCRATCHPAD    0x4e
#define THERM_CMD_CPYSCRATCHPAD  0x48
#define THERM_CMD_RECEEPROM      0xb8
#define THERM_CMD_RPWRSUPPLY     0xb4
#define THERM_CMD_SEARCHROM      0xf0
#define THERM_CMD_READROM        0x33
#define THERM_CMD_MATCHROM       0x55
#define THERM_CMD_SKIPROM        0xcc
#define THERM_CMD_ALARMSEARCH    0xec


#define THERM_DECIMAL_STEPS_12BIT 625//.0625

#define Timer1Start {TCCR1B |= (1<<CS12) | (1<<CS10);/*F_CPU/1024*/ TCNT1 = 0;}
#define Timer1Stop TCCR1B &= ~(1<<CS12) & ~(1<<CS10);
#define KonverzVarakozas_ms 1700
volatile double KonverzVarakozasCount = (double)KonverzVarakozas_ms*(double)7.2;

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <string.h>


inline __attribute__((gnu_inline)) void therm_delay(uint16_t delay){
	while(delay--) asm volatile("nop");
}

uint8_t therm_read_bit(char csatorna)
{
	switch (csatorna)
	{
		case 1:
		{
			uint8_t bit=0;

			//Pull line low for 1uS
			THERM1_LOW();
			THERM1_OUTPUT_MODE();
			therm_delay(us(1));

			//Release line and wait for 14uS
			THERM1_INPUT_MODE();
			therm_delay(us(14));

			//Read line value
			if(THERM1_PIN &(1<<THERM1_DQ)) bit=1;

			//Wait for 45uS to end and return read value
			therm_delay(us(45));
			return bit;
		}
		break;

		case 2:
		{
			uint8_t bit=0;

			//Pull line low for 1uS
			THERM2_LOW();
			THERM2_OUTPUT_MODE();
			therm_delay(us(1));

			//Release line and wait for 14uS
			THERM2_INPUT_MODE();
			therm_delay(us(14));

			//Read line value
			if(THERM2_PIN &(1<<THERM2_DQ)) bit=1;

			//Wait for 45uS to end and return read value
			therm_delay(us(45));
			return bit;
		}
		break;
	}
	return 99;
}

void therm_write_bit(uint8_t bit, char csatorna)
{
	switch (csatorna)
	{
		case 1:
		{
			//Pull line low for 1uS
			THERM1_LOW();
			THERM1_OUTPUT_MODE();

			therm_delay(us(1));

			//If we want to write 1, release the line (if not will keep low)
			if(bit) THERM1_INPUT_MODE();

			//Wait for 60uS and release the line
			therm_delay(us(60));
			THERM1_INPUT_MODE();
		}
		break;

		case 2:
		{
			//Pull line low for 1uS
			THERM2_LOW();
			THERM2_OUTPUT_MODE();

			therm_delay(us(1));

			//If we want to write 1, release the line (if not will keep low)
			if(bit) THERM2_INPUT_MODE();

			//Wait for 60uS and release the line
			therm_delay(us(60));
			THERM2_INPUT_MODE();
		}
		break;
	}
	return 99;
}

uint8_t therm_read_byte(char csatorna)
{
	cli();

	uint8_t i=8, n=0;

	while
	(i--)
	{
		//Shift one position right and store read value
		n>>=1;
		n|=(therm_read_bit(csatorna)<<7);
	}

	sei();
	return n;
}

void therm_write_byte(uint8_t byte, char csatorna)
{
	cli();

	uint8_t i=8;

	while(i--)
	{
		//Write actual bit and shift one position right to make the next bit ready
		therm_write_bit(byte&1, csatorna);
		byte>>=1;
	}

	sei();
}

uint8_t therm_reset(char csatorna)
{
	switch (csatorna)
	{
		case 1:
		{
			uint8_t i;

			//Pull line low and wait for 480uS
			THERM1_LOW();
			THERM1_OUTPUT_MODE();
			therm_delay(us(480));

			//Release line and wait for 60uS
			THERM1_INPUT_MODE();
			therm_delay(us(60));

			//Store line value and wait until the completion of 480uS period
			i=(THERM1_PIN & (1<<THERM1_DQ));
			therm_delay(us(420));

			//Return the value read from the presence pulse (0=OK, 1=WRONG)
			return i;
		}
		break;

		case 2:
		{
			uint8_t i;

			//Pull line low and wait for 480uS
			THERM2_LOW();
			THERM2_OUTPUT_MODE();
			therm_delay(us(480));

			//Release line and wait for 60uS
			THERM2_INPUT_MODE();
			therm_delay(us(60));

			//Store line value and wait until the completion of 480uS period
			i=(THERM2_PIN & (1<<THERM2_DQ));
			therm_delay(us(420));

			//Return the value read from the presence pulse (0=OK, 1=WRONG)
			return i;
		}
		break;
	}
	return 1;
}

uint8_t _crc_ibuttonDS18B20_update(uint8_t crc, uint8_t data)
{
	uint8_t i;

	crc = crc ^ data;
	for (i = 0; i < 8; i++)
	{
		if (crc & 0x01)
		crc = (crc >> 1) ^ 0x8C;
		else
		crc >>= 1;
	}

	return crc;
}
char* therm_read_temperature(/*char *homers,*/ char csatorna)
{
	// Buffer length must be at least 12bytes long! ["+XXX.XXXX C"]
	int8_t digit;
	uint16_t decimal;
	uint8_t sign;//0 = - , 1 = +
	char homers[6];

	//Reset, skip ROM and start temperature conversion
	therm_reset(csatorna);
	therm_write_byte(THERM_CMD_SKIPROM, csatorna);
	therm_write_byte(THERM_CMD_CONVERTTEMP, csatorna);

	//Wait until conversion is complete
	Timer1Start;
	while(!therm_read_bit(csatorna) && TCNT1 < KonverzVarakozasCount);
	Timer1Stop;
	
	uint8_t scratchpad[9];
	char hibaszaml;

	if(TCNT1 < KonverzVarakozasCount)
	{
		for (hibaszaml = 0; hibaszaml < 4;++hibaszaml)//próbálokások száma
		{
			//Reset, skip ROM and send command to read Scratchpad
			therm_reset(csatorna);
			therm_write_byte(THERM_CMD_SKIPROM, csatorna);
			therm_write_byte(THERM_CMD_RSCRATCHPAD, csatorna);

			int n = 0;
			for(; n < 9; ++n)
			{
				scratchpad[n] = therm_read_byte(csatorna);
			}

			uint8_t crc8 = 0;
			for (n = 0; n < 8; n++)
			{
				crc8 = _crc_ibuttonDS18B20_update(crc8, scratchpad[n]);
			}
			if(crc8 == scratchpad[8])
			{
				break;
			}

		}
		if(hibaszaml >= 4)
		{
			return "999999";
		}
		//Store sign
		sign = ~((scratchpad[1] & 0b10000000) >> 7);


		//Store temperature integer digits and decimal digits
		digit=scratchpad[0]>>4;
		digit|=(scratchpad[1]&0x7)<<4;


		//Store decimal digits
		decimal=scratchpad[0]&0xf;
		if(sign == 0)//-
		{ decimal = 16 - decimal;}
		decimal*=THERM_DECIMAL_STEPS_12BIT;



		//Format temperature into a string [+XXX.XXXX C]
		if(sign == 0)
		{
			sprintf(homers, "%d.%04u", digit, decimal);//nincs kiteve "-" a formázásban
		}
		else
		{
			sprintf(homers, "%d.%04u", digit, decimal);//nincs kiteve "+" a formázásban
		}
	}
	else
	{
		sprintf(homers, "999999");
	}
	return homers;
}


void KonfigUART()  // UART beallitasa
{
	DDRD |= (1<<PD1);
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

	KonfigUART();
	sei();
	
	while(1)
	{
		//_delay_ms(30);
		//therm_read_temperature(temp1, 1);
		//_delay_ms(30);
		//therm_read_temperature(temp2, 1);
		//_delay_ms(30);
		//
		//cli();
		//int i;
		//for (i=0; i < 6; ++i)
		//{
		//temp1Kuld[i] = temp1[i];
		//temp2Kuld[i] = temp2[i];
		//}
		//sei();
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
void UARTStringKuld(char szoveg[], int hossz)
{
	int i;
	for (i=0; i<hossz;++i)
	{
		UARTAdatKuld(szoveg[i]);
	}
}
ISR(USART_RXC_vect)
{
	if(UDR == 'c')
	{
		char szovegVissza[85];
		strcpy(szovegVissza, "a===============\n");

		strcat(szovegVissza, "a---------------:0:");
		//strcat(szovegVissza, temp1Kuld);
		strcat(szovegVissza, therm_read_temperature(1));
		strcat(szovegVissza, "\n");
		strcat(szovegVissza, "a---------------:1:");
		//strcat(szovegVissza, temp2Kuld);
		strcat(szovegVissza, therm_read_temperature(2));
		strcat(szovegVissza, "\n");


		UARTStringKuld(szovegVissza, 85);
	}

	if(UDR == 'C')
	{
		UARTAdatKuld(80);
		_delay_ms(80);
		UARTAdatKuld(100);
		_delay_ms(80);

		char szoveg[] = {'A', 'r', 'd', 'u', 'i', 'n', 'o', 'H', 'o', 's', 'z', 'e', 'n', 'z', 'o', 'r'};

		UARTStringKuld(szoveg, sizeof(szoveg)/sizeof(char));
	}
	
}

