import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from 'src/app/shared/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductDetailsService {
  baseURL = environment.apiURL;

  constructor(private httpClient: HttpClient) {}

  getProduct(id: number) {
    return this.httpClient.get<Product>(this.baseURL + 'products/' + id);
  }
}
