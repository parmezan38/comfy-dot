import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { IResponseMessage, IRoomRegister } from '../shared/interfaces';

// TODO: Change to https when changed on Api
const PROTOCOL = 'http';
const PORT = '5000';
const API = 'api';

@Injectable()
export class ApiService {

  private baseUrl: string = `${PROTOCOL}://${location.hostname}:${PORT}/${API}`;

  constructor(private http: HttpClient, private auth: AuthService) { }

  public listRooms(): Observable<any> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'json' };
    return this.http.get<any>(`${this.baseUrl}/rooms`, options)
      .pipe(catchError(this.handleError));
  }

  public createRoom(room: IRoomRegister): Observable<IResponseMessage> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'json' };
    return this.http.post<IResponseMessage>(`${this.baseUrl}/rooms`, room, options)
      .pipe(catchError(this.handleError));
  }

  public deleteRoom(id: number): Observable<IResponseMessage> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'json' };
    return this.http.delete<IResponseMessage>(`${this.baseUrl}/rooms/${id}`, options)
      .pipe(catchError(this.handleError));
  }

  private getHeaders(): any {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.auth.getAccessToken()}`,
      'X-Requested-With': 'XMLHttpRequest'
    });
  }

  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if (error.error instanceof Error) {
      let errMessage = error.error.message;
      return Observable.throw(errMessage);
    }
    return Observable.throw(error || 'Server error');
  }

}
