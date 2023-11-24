import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorsComponent } from './errors/errors.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';

@NgModule({
  declarations: [ErrorsComponent, NotFoundComponent, ServerErrorComponent],
  imports: [CommonModule],
})
export class TestModule {}
