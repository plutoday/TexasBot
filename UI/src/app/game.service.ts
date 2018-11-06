import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { NewRoundRequest } from './contract/new-round-request';
import { NewRoundResponse } from './contract/new-round-response';
import { Observable } from 'rxjs/Observable';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import { RoundStatus } from './models/round-status';
import { HeroHolesRequest } from './contract/hero-holes-request';
import { HeroHolesResponse } from './contract/hero-holes-response';
import { NotifyDecisionRequest } from './contract/notify-decision-request';
import { NotifyDecisionResponse } from './contract/notify-decision-response';
import { GetDecisionResponse } from './contract/get-decision-response';
import { FlopsRequest } from './contract/flops-request';
import { FlopsResponse } from './contract/flops-response';
import { TurnResponse } from './contract/turn-response';
import { TurnRequest } from './contract/turn-request';
import { RiverRequest } from './contract/river-request';
import { RiverResponse } from './contract/river-response';

@Injectable()
export class GameService {

  host: string = "http://localhost:53598";
  startNewRoundUrl: string = this.host + "/Rounds/StartNewRound";
  notifyHeroHolesUrl: string = this.host + "/Rounds/NotifyHeroHoles";
  notifyDecisionUrl: string = this.host + "/Rounds/NotifyDecision";
  getDecisionUrl: string = this.host + "/Rounds/GetDecision";
  notifyFlopsUrl: string = this.host + "/Rounds/NotifyFlops";
  notifyTurnUrl: string = this.host + "/Rounds/NotifyTurn";
  notifyRiverUrl: string = this.host + "/Rounds/NotifyRiver";

  headers: HttpHeaders = new HttpHeaders().set("Content-Type", "application/json")
  .set("Accept", "application/json");

  constructor(private http: HttpClient) {}

  startNewRound(request: NewRoundRequest): Observable<NewRoundResponse> {
    console.log('HeroIndex in the request is ' + request.HeroIndex);
    console.log('sending new round request ' + this.startNewRoundUrl + ' body is ' + JSON.stringify(request));
    let headers = this.headers;
    let response = this.http.post<NewRoundResponse>(this.startNewRoundUrl, JSON.stringify(request), {headers});
    console.log('new round response: ' + JSON.stringify(response));
    return response;
  }

  notifyHeroHoles(request: HeroHolesRequest): Observable<HeroHolesResponse>{
    let headers = this.headers;
    let response = this.http.post<HeroHolesResponse>(this.notifyHeroHolesUrl, JSON.stringify(request), {headers});
    return response;
  }

  notifyFlops(request: FlopsRequest): Observable<FlopsResponse>{
    let headers = this.headers;
    let response = this.http.post<FlopsResponse>(this.notifyFlopsUrl, JSON.stringify(request), {headers});
    return response;
  }
  
  notifyTurn(request: TurnRequest): Observable<TurnResponse>{
    let headers = this.headers;
    let response = this.http.post<TurnResponse>(this.notifyTurnUrl, JSON.stringify(request), {headers});
    return response;
  }

  notifyRiver(request: RiverRequest): Observable<RiverResponse>{
    let headers = this.headers;
    let response = this.http.post<RiverResponse>(this.notifyRiverUrl, JSON.stringify(request), {headers});
    return response;
  }

  notifyDecision(request: NotifyDecisionRequest): Observable<NotifyDecisionResponse>{
    let headers = this.headers;
    let response = this.http.post<NotifyDecisionResponse>(
      this.notifyDecisionUrl, JSON.stringify(request), {headers});    
    return response;
  }

  getDecision(roundId: string): Observable<GetDecisionResponse> {
    let headers: HttpHeaders = new HttpHeaders().set("Content-Type", "application/json")
    .set("Accept", "application/json").set("roundId", roundId);
    let response = this.http.get<GetDecisionResponse>(this.getDecisionUrl, {headers});
    return response;
  }
}
