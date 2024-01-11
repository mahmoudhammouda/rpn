import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-app-calculator',
  templateUrl: './app-calculator.component.html',
  styleUrls: ['./app-calculator.component.css']
})
export class AppCalculatorComponent implements OnInit {

  stackDisplay:string | undefined;
  currentNumber:string | undefined;


  //currentNumber = '0';
  firstOperand = null;
  operator = null;
  waitForSecondNumber = false;
  
  constructor() { }

  ngOnInit() {
    this.stackDisplay = "";
    this.currentNumber = '0';
  }


  public getNumber(v: string){
    console.log(v);
    if(this.waitForSecondNumber)
    {
      this.currentNumber = v;
      this.waitForSecondNumber = false;
    }else{
      this.currentNumber === '0'? this.currentNumber = v: this.currentNumber += v;

    }
  }

  public push(){

    if(this.currentNumber != null && this.currentNumber != undefined){
      this.stackDisplay += this.currentNumber;
      this.stackDisplay += ' ';
      this.currentNumber='0';
    }
  
  }

  /*getDecimal(){
    if(!this.currentNumber.includes('.')){
        this.currentNumber += '.'; 
    }
  }*/
/*
  private doCalculation(op , secondOp){
    switch (op){
      case '+':
      return this.firstOperand += secondOp; 
      case '-': 
      return this.firstOperand -= secondOp; 
      case '*': 
      return this.firstOperand *= secondOp; 
      case '/': 
      return this.firstOperand /= secondOp; 
      case '=':
      return secondOp;
    }
  }*/

  public getOperation(op: string){}
  /*public getOperation(op: string){
    console.log(op);

    if(this.firstOperand === null){
      this.firstOperand = Number(this.currentNumber);

    }else if(this.operator){
      const result = this.doCalculation(this.operator , Number(this.currentNumber))
      this.currentNumber = String(result);
      this.firstOperand = result;
    }
    this.operator = op;
    this.waitForSecondNumber = true;

    console.log(this.firstOperand);
 
  }*/

  public clear(){
    this.currentNumber = '0';
    this.firstOperand = null;
    this.operator = null;
    this.waitForSecondNumber = false;
  }

}
