import { Component, OnInit, Input } from '@angular/core';
import { Card } from '../models/card';
import { DataSharingService } from '../data-sharing-service';

@Component({
  selector: 'app-new-community',
  templateUrl: './new-community.component.html',
  styleUrls: ['./new-community.component.css']
})
export class NewCommunityComponent implements OnInit {

  Flop1: Card = new Card();
  Flop2: Card = new Card();
  Flop3: Card = new Card();
  Turn: Card = new Card();
  River: Card = new Card();

  constructor(private dataSharingService: DataSharingService) { }

  ngOnInit() {
    this.dataSharingService.currentStatus.subscribe(status => {
      if (status.flop1 != undefined){
        this.Flop1 = status.flop1;
      }
      if (status.flop2 != undefined){
        this.Flop2 = status.flop2;
      }
      if (status.flop3 != undefined){
        this.Flop3 = status.flop3;
      }
      if (status.turn != undefined){
        this.Turn = status.turn;
      }
      if (status.river != undefined){
        this.River = status.river;
      }
    });
  }

}
