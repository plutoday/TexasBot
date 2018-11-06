import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { SeatComponent } from './seat/seat.component';
import { TableComponent } from './table/table.component';
import { ConsoleComponent } from './console/console.component';
import { CardComponent } from './card/card.component';
import {HttpClientModule} from '@angular/common/http';
import { NewGameFormComponent } from './form/new-game-form/new-game-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GameService } from './game.service';
import { DataSharingService } from './data-sharing-service';
import { DecisionFormComponent } from './form/decision-form/decision-form.component';
import { HeroDecisionComponent } from './hero-decision/hero-decision.component';
import { NewTableComponent } from './new-table/new-table.component';
import { NewSeatComponent } from './new-seat/new-seat.component';
import { NewCommunityComponent } from './new-community/new-community.component';
import { NewDigitControlComponent } from './new-digit-control/new-digit-control.component';
import { NewDigitPanelComponent } from './new-digit-panel/new-digit-panel.component';


@NgModule({
  declarations: [
    AppComponent,
    SeatComponent,
    TableComponent,
    ConsoleComponent,
    CardComponent,
    NewGameFormComponent,
    DecisionFormComponent,
    HeroDecisionComponent,
    NewTableComponent,
    NewSeatComponent,
    NewCommunityComponent,
    NewDigitControlComponent,
    NewDigitPanelComponent,
  ],

  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  
  providers: [GameService, DataSharingService],
  
  bootstrap: [AppComponent]
})
export class AppModule { }
