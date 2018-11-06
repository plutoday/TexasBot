import { Component, OnInit } from '@angular/core';
import { SeatComponent } from '../seat/seat.component';
import { Card } from '../models/card';
import { DataSharingService } from '../data-sharing-service';
import {RoundStatus} from '../models/round-status';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})

export class TableComponent implements OnInit{
  
  roundId: string;

  Seats: Array<SeatComponent> = new Array<SeatComponent>(
    new SeatComponent(),
    new SeatComponent(),
    new SeatComponent(),
    new SeatComponent(),
    new SeatComponent(),
    new SeatComponent(),
  );

  Flop1: Card;
  Flop2: Card;
  Flop3: Card;
  Turn: Card;
  River: Card;

  heroIndex: number;

  constructor(private dataSharingService: DataSharingService) {    
  }

  ngOnInit(): void {

    this.dataSharingService.roundSetup.subscribe(setup => {
      if (setup.heroIndex != undefined) {
        console.log('heroIndex is ' + setup.heroIndex);         

        this.Seats[setup.heroIndex].IsHero = true;
        this.heroIndex = setup.heroIndex;
      }

    });


    this.dataSharingService.currentStatus.subscribe(status =>{
      this.roundId = status.roundId;

      if (status.heroHole1 != undefined && status.heroHole2 != undefined){
        console.log('hole1 is ' + status.heroHole1 + ' hole2 is ' + status.heroHole2);
        this.Seats[this.heroIndex].Hole1 = status.heroHole1;        
        this.Seats[this.heroIndex].Hole2 = status.heroHole2;
        this.Seats[this.heroIndex].ReceivedHoles = true;
      }

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
