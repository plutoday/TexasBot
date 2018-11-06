import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Card } from '../models/card';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})

export class CardComponent {
  @Input("card") card: Card = new Card();
  @Input("type") type: string;
  @Output("clicked") clicked = new EventEmitter();
  
  constructor(){
    this.card = new Card();
  }

  onClick(){
    console.log("card.component.onClick()");
    this.card.onConsoleClick();
    this.clicked.emit(this.card);
  }

  isConsoleCard() : boolean{
    return this.type == 'console';
  }

  isTableCard() : boolean{
    return this.type == 'table';
  }

  isCommunityCard(): boolean{
    return this.type == 'community';
  }

  isUsed() : boolean{
    return this.card.Used;
  }
}
