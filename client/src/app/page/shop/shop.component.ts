import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/shared/product';
import { ShopService } from './shop.service';
import { ProductBrand } from 'src/app/shared/brand';
import { ProductType } from 'src/app/shared/type';
import { ShopParams } from 'src/app/shared/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: Product[] = [];
  productBrands: ProductBrand[] = [];
  productTypes: ProductType[] = [];
  shopParams: ShopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'price' },
    { name: 'Price: High to Low', value: '-price' },
  ];
  totalItems: number = 0;

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProductBrands();
    this.getProductTypes();
    this.getProducts();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalItems = response.totalItems;
        this.products = response.data;
      },
      error: (err) => console.log(err),
    });
  }

  getProductBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) =>
        (this.productBrands = [{ id: 0, name: 'All' }, ...response]),
      error: (err) => console.log(err),
    });
  }

  getProductTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) =>
        (this.productTypes = [{ id: 0, name: 'All' }, ...response]),
      error: (err) => console.log(err),
    });
  }

  onBrandIdSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeIdSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(event: any) {
    this.shopParams.sort = event.target.value;
    this.getProducts();
  }

  onPageChanged(event: any) {
    if (this.shopParams.pageNumber !== event) {
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch() {
    this.shopParams.search = this.searchTerm?.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    if(this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
