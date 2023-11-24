import { HttpClient, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css'],
})
export class ErrorsComponent {
  baseURL = environment.apiURL;
  validationErrors: string[] = [];

  constructor(private httpClient: HttpClient) {}

  get404Error() {
    this.httpClient.get(this.baseURL + 'products/42').subscribe({
      next: (response) => console.log(response),
      error: (err) => console.log(err),
    });
  }

  get500Error() {
    this.httpClient.get(this.baseURL + 'buggy/server-error').subscribe({
      next: (response) => console.log(response),
      error: (err) => console.log(err),
    });
  }

  get400Error() {
    this.httpClient.get(this.baseURL + 'buggy/bad-request').subscribe({
      next: (response) => console.log(response),
      error: (err) => console.log(err),
    });
  }

  get400ValidationError() {
    let params = new HttpParams();
    params = params.append('brandId', 'one');
    this.httpClient.get(this.baseURL + 'products', { params }).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        console.log(err);
        this.validationErrors = err.errors;
      },
    });
  }
}
