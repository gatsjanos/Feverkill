/*
 * GccApplication3.c
 * Created: 2015.05.21. 20:39:40
 * Author: Gats János
 
 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 ~~~~~FIGYELEM!!!!!!!!!!#!#!#!#!#!#!#!#!#!#!#!#~~~~~
 ~~~~~KAPCSOLD KI A "CKDIV8" FUSE BITET!!!!!!!!~~~~~
 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 */ 
#include <avr/io.h>
#include <avr/interrupt.h>
#include <compat/twi.h>

#define SCL_CLOCK 10000
#define TWIprescaler_value 1
#define TWIBaudRate ((F_CPU/SCL_CLOCK)-16)/(2*TWIprescaler_value)

#define F_CPU 7372800

volatile char hanyadik = 0;
ISR(TWI_vect)
{
	if(TWSR == 0x60) // IF slave address + w received
	{  
		hanyadik = 0;              
	}
	
	if(TWSR == 0x80) // IF data received
	{            
		++hanyadik;
		if(hanyadik == 1)//2
			{OCR0A = TWDR + 155; if(TWDR == 0){TCCR0A &= ~(1<<COM0A1);} else {TCCR0A |= (1<<COM0A1);} }
		else if(hanyadik == 2)//3
			{OCR1A = TWDR + 155; if(TWDR == 0){TCCR1A &= ~(1<<COM1A1);} else {TCCR1A |= (1<<COM1A1);} }
		else if(hanyadik == 3)//4
			{OCR2A = TWDR + 155; if(TWDR == 0){TCCR2A &= ~(1<<COM2A1);} else {TCCR2A |= (1<<COM2A1);} }
		else if(hanyadik == 4)//5
			{OCR2B = TWDR + 155; if(TWDR == 0){TCCR2A &= ~(1<<COM2B1);} else {TCCR2A |= (1<<COM2B1);} }
		else if(hanyadik == 5)//6
			{OCR1B = TWDR + 155; if(TWDR == 0){TCCR1A &= ~(1<<COM1B1);} else {TCCR1A |= (1<<COM1B1);} }
		else if(hanyadik == 6)//7
			{OCR0B = TWDR + 155; if(TWDR == 0){TCCR0A &= ~(1<<COM0B1);} else {TCCR0A |= (1<<COM0B1);} }
	}
	TWCR |= 0x80;// Clear interrupt flag bit
}
ISR(TIMER0_OVF_vect)
{
	TCNT0 = 155;
}
ISR(TIMER1_OVF_vect)
{
	TCNT1 = 155;
}
ISR(TIMER2_OVF_vect)
{
	TCNT2 = 155;
}
int main(void)
{
	hanyadik = 0;
	sei();
	OC_init();
	
	TWBR = TWIBaudRate;
	
	TWAR = 0xA0;                // Slave address = 0xA0
	TWCR = 0x45;                // Enable TWI, interrupt enable
    
	
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
	
	TCCR0A |= (1<<COM0A1) | (1<<COM0B1) | (1<<WGM01) | (1<<WGM00);
	TCCR0B |= (1<<CS00);
	OCR0A = 0;
	OCR0B = 0;
	
	TCCR1A |= (1<<COM1A1) | (1<<COM1B1) | (1<<WGM10);
	TCCR1B |= (1<<WGM12) | (1<<CS10);
	OCR1B = 0;
	OCR1A = 0;
	
	TCCR2A |= (1<<COM2A1) | (1<<COM2B1) | (1<<WGM21) | (1<<WGM20);
	TCCR2B |= (1<<CS20);
	OCR2A = 0;
	OCR2B = 0;

	TIMSK0 |= (1<<TOIE0);
	TIMSK1 |= (1<<TOIE1);
	TIMSK2 |= (1<<TOIE2);
}