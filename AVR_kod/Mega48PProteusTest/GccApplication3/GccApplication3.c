/*
 * GccApplication3.c
 * Created: 2015.05.21. 20:39:40
 * Author: Gats János
 
 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 ~~~~~FIGYELEM!!!!!!!!!!#!#!#!#!#!#!#!#!#!#!#!#~~~~~
 ~~~~~KAPCSOLD KI A "CKDIV8" FUSE BITET!!!!!!!!~~~~~
 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 */
#define F_CPU 7372800

#include <avr/io.h>
#include <avr/interrupt.h>
#include <compat/twi.h>
#include <avr/pgmspace.h>

///////TWI/////
volatile char TWICim = 0xA0;
#define SCL_CLOCK 10000
#define TWIprescaler_value 1
#define TWIBaudRate ((F_CPU/SCL_CLOCK)-16)/(2*TWIprescaler_value)
volatile char TWISlaveKozpontCim = (50<<1);
volatile char TWISlaveCim;
#define TWI_IR 0
#define TWI_OLVAS 1
////////////////

///////TWI bufferek/////
volatile char TWIkuldendo[20];
volatile char TWIkuldendoHanyadik = 0, TWIkuldendoHossz = 0;
volatile char TWIvisszaolvasson = 0;

volatile char TWIbejovo[20];
volatile char TWIbejovoHossz = 0, TWIbejovoHanyadik = 0, TWIujbejovo = 0;
volatile char TWIkuldjvissza = 0;

volatile char twiRepstartolt = 0;

int main(void)
{
	OC_init();
	
    TWAR = TWICim;
    TWBR = TWIBaudRate;
    TWCR = (1<<TWEN) | (1<<TWEA)| (1<<TWIE);
	
	sei();

	//////////////////////////////////////////////////////////////////////////////////
	//ADCSRA |= (1<<ADEN) | (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2) | (1<<ADSC);
	//char csatorna = 0;
	//ADMUX = 0b00000000 | (1<<ADLAR) | csatorna;
	//////////////////////////////////////////////////////////////////////////////////
	
	while(1)
    {
		//////////////////////////////////////////////////////////////////////////////////
		//ADCSRA |= (1<<ADEN) | (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2) | (1<<ADSC);
		//OCR0A = OCR0B = OCR1A = OCR1B = OCR2A = OCR2B = TWDR;
		//////////////////////////////////////////////////////////////////////////////////
		
    }
}

void OC_init()
{
	DDRB |= (1<<PINB1) | (1<<PINB2) | (1<<PINB3);
	DDRD |= (1<<PIND3) | (1<<PIND5) |(1<<PIND6);
	
	//TCCR0A |= (1<<COM0A1) | (1<<COM0B1) | (1<<WGM01) | (1<<WGM00);
	TCCR0B |= (1<<CS00);
	OCR0A = 0;
	OCR0B = 0;
	
	//TCCR1A |= (1<<COM1A1) | (1<<COM1B1) | (1<<WGM10);
	TCCR1B |= (1<<WGM12) | (1<<CS10);
	OCR1B = 0;
	OCR1A = 0;
	
	//TCCR2A |= (1<<COM2A1) | (1<<COM2B1) | (1<<WGM21) | (1<<WGM20);
	TCCR2B |= (1<<CS20);
	OCR2A = 0;
	OCR2B = 0;
}

unsigned short const crc16table[256] PROGMEM = {
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
		//crc16 = _crc_ccitt_update(crc16, adat[i]);//Belső táblás
		crc16 = (crc16 >> 8) ^ pgm_read_dword(&crc16table[(crc16 ^ adat[i]) & 0xff]);//saját táblás

		//data = pgm_read_byte(&crc16table[(crc16 ^ adat[i]) & 0xff]);
		//crc16 = (crc16 >> 8) ^ crc16table[(crc16 ^ adat[i]) & 0xff];//saját táblás
	}

	return crc16;
}
#pragma region TWI kezeles
void twi_master_start()
{
	twiRepstartolt = 0;
	// Enable interrupt and start bits
	TWCR = (1 << TWINT)| (1 << TWEN) | (1 << TWSTA) | (1<<TWIE);
}
void twi_master_Repstart()
{
	twiRepstartolt = 1;
	// Enable interrupt and start bits
	TWCR = (1 << TWINT)| (1 << TWEN) | (1 << TWSTA) | (1<<TWIE);
}

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
	twi_master_start();
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
					twi_master_start();
				}
				else
				twi_master_stop();
			}
			else
			{
				TWDR = TWIkuldendo[TWIkuldendoHanyadik];
				++TWIkuldendoHanyadik;
				twi_setAck();
			}
			break;
		}
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
					twi_master_Repstart();
				}
				else
				{
					twi_master_stop();
				}
			}
			break;
		}
		case 0x30://Data byte transmitted; NOT ACK received
		{
			twi_master_stop();
			break;
		}
		case 0x38://Arbitration lost in SLA+W or data bytes
		{

			break;
		}
		//////////////////////MASTER RECEIVER///////////////////////
		case 0x40://SLA+R transmitted; ACK received
		{
			TWIbejovoHanyadik = 0;
			TWIujbejovo = 0;
			twi_setAck();
			break;
		}
		case 0x48://SLA+R transmitted; NOT ACK received
		{
			break;
		}
		case 0x50://Data byte received; ACK returned
		{
			if(TWIbejovoHanyadik == 0)
			{
				TWIbejovoHossz = TWDR;
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
		case 0x58://Data byte received; NOT ACK returned
		{
			
			break;
		}
	}
	//////////////////////SLAVE RECEIVER///////////////////////
	switch (TWSR & 0b11111000)
	{
		case 0x60://Own SLA+W  received; ACK  returned
		{
			TWIbejovoHanyadik = 0;
			TWIujbejovo = 0;
			twi_setAck();
			break;
		}
		case 0x68://Arbitration lost in SLA+R/W as Master; own SLA+W  received; ACK returned
		{

			break;
		}
		case 0x70://General call address received; ACK returned
		{

			break;
		}
		case 0x78://Arbitration lost in SLA+R/W as Master; General call address received; ACK returned
		{

			break;
		}
		case 0x80://Previously addressed with own SLA+W; data received; ACK  returned
		{
			if(TWIbejovoHanyadik == 0)
			{
				TWIbejovoHossz = TWDR;
			}

			if(TWIbejovoHanyadik < TWIbejovoHossz)
			{
				TWIbejovo[TWIbejovoHanyadik] = TWDR;
				//twi_setAck();
			}
			
			++TWIbejovoHanyadik;

			if(TWIbejovoHanyadik >= TWIbejovoHossz)
			{
				TWIujbejovo = 1;
			}
			break;
		}
		case 0x88://Previously addressed with own SLA+W; data received; NOT ACK  returned
		{

			break;
		}
		case 0x90://Previously addressed with general call; data received; ACK  returned
		{

			break;
		}
		case 0x98://Previously addressed with general call; data received; NOT ACK returned
		{

			break;
		}
		case 0xA0://A STOP condition or repeated START condition received while still addressed as Slave
		{

			break;
		}
		//////////////////////SLAVE TRANSMITTER///////////////////////
		case 0xA8://Own SLA+R received; ACK returned
		{
			TWIkuldjvissza = 1;

			if(TWIbejovo[0] > 1)
			{
				char behossz = TWIbejovoHossz;
				char beuzenet[behossz];
				int i;
				for(i = 0; i < behossz; ++i)
				{
					beuzenet[i] = TWIbejovo[i];
				}
				switch(TWIbejovo[1])
				{
					case 20://Bejövő fordszámok
					{
						unsigned short crc_Be = beuzenet[behossz-2] | (beuzenet[behossz-1] << 8);
						unsigned short crc16 = Crc16Szamolo(beuzenet, behossz - 2);

						if(crc16 == crc_Be)
						{
							
							{OCR0A = TWIbejovo[2]; if(TWIbejovo[2] == 0){TCCR0A &= ~(1<<COM0A1);} else {TCCR0A |= (1<<COM0A1);} }//2
							{OCR1A = TWIbejovo[3]; if(TWIbejovo[3] == 0){TCCR1A &= ~(1<<COM1A1);} else {TCCR1A |= (1<<COM1A1);} }//3
							{OCR2A = TWIbejovo[4]; if(TWIbejovo[4] == 0){TCCR2A &= ~(1<<COM2A1);} else {TCCR2A |= (1<<COM2A1);} }//4
							{OCR2B = TWIbejovo[5]; if(TWIbejovo[5] == 0){TCCR2A &= ~(1<<COM2B1);} else {TCCR2A |= (1<<COM2B1);} }//5
							{OCR1B = TWIbejovo[6]; if(TWIbejovo[6] == 0){TCCR1A &= ~(1<<COM1B1);} else {TCCR1A |= (1<<COM1B1);} }//6
							{OCR0B = TWIbejovo[7]; if(TWIbejovo[7] == 0){TCCR0A &= ~(1<<COM0B1);} else {TCCR0A |= (1<<COM0B1);} }//7

							TWIkuldendo[0] = TWIkuldendoHossz = 2;
							TWIkuldendo[1] = 17;//Helyes CRC
							
							TWIkuldendoHanyadik = 1;
							TWDR = TWIkuldendo[0];
						}
						else
						{
							TWIkuldendo[0] = TWIkuldendoHossz = 2;
							TWIkuldendo[1] = 33;//Helytelen CRC
							
							TWIkuldendoHanyadik = 1;
							TWDR = TWIkuldendo[0];
						}

						break;
					}
					default:
					{
						TWIkuldendo[0] = TWIkuldendoHossz = 2;
						TWIkuldendo[1] = 33;//Helytelen kód
						
						TWIkuldendoHanyadik = 1;
						TWDR = TWIkuldendo[0];
					}
				}
			}
			break;
		}
		case 0xB0://Arbitration lost in SLA+R/W as Master; own SLA+R received; ACK returned
		{

			break;
		}
		case 0xB8://Data byte in TWDR transmitted; ACK received
		{
			if(TWIkuldjvissza)
			{
				if(TWIkuldendoHanyadik < TWIkuldendoHossz)
				{
					TWDR = TWIkuldendo[TWIkuldendoHanyadik];

					if(TWIkuldendoHanyadik == TWIkuldendoHossz -1)
					{
						twi_setNack();
					}
					++TWIkuldendoHanyadik;
				}
				else
				{
					TWDR=255;
				}
			}
			else
			{
				TWDR=255;
			}
			break;
		}
		case 0xC0://Data byte in TWDR transmitted; NOT ACK received
		{

			break;
		}
		case 0xC8://Last data byte in TWDR transmitted (TWEA = “0”); ACK received
		{
			twi_setAck();
			break;
		}
	}

	TWCR |= 0x80;// Clear interrupt flag bit
}

#pragma endregion TWI kezeles