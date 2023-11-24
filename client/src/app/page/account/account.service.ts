import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaySubject, map, of } from 'rxjs';
import { User } from 'src/app/shared/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseURL = environment.apiURL;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private httpClient: HttpClient, private router: Router) {}

  loadCurrentUser(token: string | null) {
    if (!token) {
      this.currentUserSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.httpClient
      .get<User>(this.baseURL + 'account', { headers })
      .pipe(
        map((user) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
            return user;
          }
          return null;
        })
      );
  }

  login(values: any) {
    return this.httpClient
      .post<User>(this.baseURL + 'account/login', values)
      .pipe(
        map((user) => {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        })
      );
  }

  register(values: any) {
    return this.httpClient
      .post<User>(this.baseURL + 'account/register', values)
      .pipe(
        map((user) => {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.httpClient.get<boolean>(
      this.baseURL + 'account/emailExists?email=' + email
    );
  }
}
