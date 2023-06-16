import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    //BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-righ',
    }),
  ],
  exports: [
    //BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }
