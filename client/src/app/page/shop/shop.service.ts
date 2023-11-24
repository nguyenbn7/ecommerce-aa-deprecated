import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductBrand } from 'src/app/shared/brand';
import { Pagination } from 'src/app/shared/pagination';
import { Product } from 'src/app/shared/product';
import { ShopParams } from 'src/app/shared/shopParams';
import { ProductType } from 'src/app/shared/type';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseURL = environment.apiURL;
  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId > 0)
      params = params.append('brandId', shopParams.brandId);
    if (shopParams.typeId > 0)
      params = params.append('typeId', shopParams.typeId);
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber);
    params = params.append('pageSize', shopParams.pageSize);
    if (shopParams.search) params = params.append('search', shopParams.search);

    return this.http.get<Pagination<Product>>(this.baseURL + 'products', {
      params,
    });
  }

  getBrands() {
    return this.http.get<ProductBrand[]>(this.baseURL + 'products/brands');
  }

  getTypes() {
    return this.http.get<ProductType[]>(this.baseURL + 'products/types');
  }
}
