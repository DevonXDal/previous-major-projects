import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap, catchError, throwError, lastValueFrom } from 'rxjs';
import { Constants } from '../constants';
import { WaitStaffStatus } from '../wait/data/wait-staff-status';
import { KitchenStaffStatus } from './data/kitchen-staff-status';

@Injectable({
  providedIn: 'root'
})
export class KitchenStaffApiService {

  _currStatus?: KitchenStaffStatus; // This will always reflect whether the recent data came in
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  public get currStatus() {
    return this._currStatus;
  }

  constructor(private http: HttpClient) { }

  fetchStatus(): Observable<KitchenStaffStatus> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/KitchenStaff/Status`

    return this.http.get<KitchenStaffStatus>(url)
      .pipe(
        tap(data => this._currStatus = data),
        catchError(err => this.handleError(err))
      );
  }

  async markOrderAsStarted(orderGuid: string): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/KitchenStaff/DeclareOrderIsStarted`

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;
    formData.append('orderGuid', orderGuid);

    try {
      response = await lastValueFrom(this.http.post<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null})
    } catch (err) {
      response = err as HttpErrorResponse;
    }
    
    
    if (!response) return Promise.resolve(Constants.noResponseFromApi);
    if (response.status == 200) return Promise.resolve('');
    
    return Promise.resolve(`Failed to to mark order ${orderGuid} as having been started`); // Fail message
  }

  async markOrderAsFinished(orderGuid: string): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/KitchenStaff/DeclareOrderIsReadyForServing`

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;
    formData.append('orderGuid', orderGuid);

    try {
      response = await lastValueFrom(this.http.post<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null})
    } catch (err) {
      response = err as HttpErrorResponse;
    }
    
    
    if (!response) return Promise.resolve(Constants.noResponseFromApi);
    if (response.status == 200) return Promise.resolve('');
    
    return Promise.resolve(`Failed to to mark order ${orderGuid} as having been started`); // Fail message
  }

  private handleError(err: any): Observable<never> {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      errorMessage = `Backend returned code ${err.status}: ${err.body.error} for Kitchen Staff`;
    }
    console.error(err);
    return throwError(() => errorMessage);
  }
}
