import { Component, OnInit, Input } from '@angular/core';
import { DataSharingService } from '../data-sharing-service';

@Component({
  selector: 'app-new-digit-control',
  templateUrl: './new-digit-control.component.html',
  styleUrls: ['./new-digit-control.component.css']
})
export class NewDigitControlComponent implements OnInit {

  @Input('index') index: number;  

  digitSelectedMarks: Array<boolean>;

  constructor(private dataSharingService: DataSharingService) { }

  ngOnInit() {
    this.digitSelectedMarks = new Array<boolean>();
    this.digitSelectedMarks.push(true);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);
    this.digitSelectedMarks.push(false);

    this.dataSharingService.currentStatus.subscribe(status => {
      this.setDigit(status.digits[this.index]);
    });

  }

  onDigitClick(i: number){
    this.setDigit(i);
    this.dataSharingService.setChipsDigit(this.index, i);
  }

  setDigit(i: number){
    this.digitSelectedMarks.forEach((item, index) => {
      if (index == i){
        this.digitSelectedMarks[index] = true;
      }
      else{
        this.digitSelectedMarks[index] = false;
      }
  });
  }
}
