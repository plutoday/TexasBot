import { Component, OnInit } from '@angular/core';
import { DataSharingService } from '../data-sharing-service';

@Component({
  selector: 'app-new-digit-panel',
  templateUrl: './new-digit-panel.component.html',
  styleUrls: ['./new-digit-panel.component.css']
})
export class NewDigitPanelComponent implements OnInit {

  digits: Array<number>;

  constructor(private dataSharingService: DataSharingService) { }

  ngOnInit() {
    this.dataSharingService.currentStatus.subscribe(status => {
      this.digits = status.digits;
    });
  }

}
