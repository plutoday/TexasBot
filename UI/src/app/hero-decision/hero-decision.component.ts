import { Component, OnInit } from '@angular/core';
import { Decision } from '../contract/decision';
import { DataSharingService } from '../data-sharing-service';

@Component({
  selector: 'app-hero-decision',
  templateUrl: './hero-decision.component.html',
  styleUrls: ['./hero-decision.component.css']
})
export class HeroDecisionComponent implements OnInit {

  decision: Decision;
  decisionReceived: boolean = false;

  constructor(private dataSharingService: DataSharingService) {

   }

  ngOnInit() {
    this.dataSharingService.currentStatus.subscribe(status => {
      if (status.heroDecision != undefined){
        this.decision = status.heroDecision;
        this.decisionReceived = true;
      }
    });
  }

}
