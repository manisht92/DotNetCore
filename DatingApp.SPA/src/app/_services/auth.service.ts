import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:5001/api/auth/';
  userToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {
    const header = new HttpHeaders({'content-type': 'application/json'});
    const options = {headers: header};

    return this.http.post(this.baseUrl + 'login', model, this.requestOptions()).pipe(map((response: Response) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user['tokenString']);
        this.userToken = user['tokenString'];
      }
      return response;
    }));
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model, this.requestOptions());
  }

  private requestOptions() {
    const header = new HttpHeaders({'content-type': 'application/json'});
    return {headers: header};
  }

}
