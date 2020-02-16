import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { IRegisterResponse, IRoomRegister } from '../shared/interfaces';

// TODO: Change to https when changed on Api
const PROTOCOL = 'http';
const PORT = '5000';
const API = 'api';

@Injectable()
export class ApiService {

  private baseUrl: string = `${PROTOCOL}://${location.hostname}:${PORT}/${API}`;

  constructor(private http: HttpClient, private authService: AuthService) { }

  public listRooms(): Observable<string> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'json' };
    return this.http.get<string>(`${this.baseUrl}/rooms`, options)
      .pipe(catchError(this.handleError));
  }

  public createRoom(room: IRoomRegister): Observable<IRegisterResponse> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'text' };
    return this.http.post<IRegisterResponse>(`${this.baseUrl}/rooms`, room, options)
      .pipe(catchError(this.handleError));
  }

  public deleteRoom(id: number): Observable<IRegisterResponse> {
    const headers = this.getHeaders();
    const options: Object = { headers, responseType: 'text' };
    return this.http.delete<IRegisterResponse>(`${this.baseUrl}/rooms/${id}`, options)
      .pipe(catchError(this.handleError));
  }

  private getHeaders(): any {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.getAccessToken()}`,
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
