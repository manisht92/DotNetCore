import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:5001/api/auth/';
  userToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model, this.requestOptions()).pipe(map((response: Response) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user['tokenString']);
        this.userToken = user['tokenString'];
      }
      return response;
    }), catchError(this.handleError));
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model, this.requestOptions()).pipe(catchError(this.handleError));
  }

  private requestOptions() {
    const header = new HttpHeaders({'content-type': 'application/json'});
    return {headers: header};
  }

  private handleError(error: any) {
    console.log('=0=');
    console.log(error);
    console.log('=1=');
    const applicationError = error.headers.get('Application-Error');

    console.log('applicationError=');
    console.log(applicationError);

    if (applicationError) {
      return throwError(applicationError);
    }

    const serverError = error.json();
    console.log('serverError=');
    console.log(serverError);

    let modelStateErrors = '';
    if (serverError) {
      for (const key in serverError) {
        if (serverError[key]) {
          modelStateErrors += serverError[key] + '\n';
        }
      }
    }
    console.log('modelStateErrors=');
    console.log(modelStateErrors);
    return throwError(
      modelStateErrors || 'Server error'
    );
  }
}
