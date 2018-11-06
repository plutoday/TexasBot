import { Component, OnInit, Input } from '@angular/core';
import {CardComponent} from '../card/card.component';
import { Card } from '../models/card';

@Component({
  selector: 'app-seat',
  templateUrl: './seat.component.html',
  styleUrls: ['./seat.component.css']
})
export class SeatComponent implements OnInit {

  @Input('Hole1') Hole1: Card = new Card();
  @Input('Hole2') Hole2: Card = new Card();

  @Input('PlayerName') PlayerName: string;
  @Input('StackSize') StackSize: number;
  IsHero: boolean;
  ReceivedHoles: boolean;

  ngOnInit() {
  }
}
