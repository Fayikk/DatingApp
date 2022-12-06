import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:"toast-bottom-right"
    }),
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    
  ],
  exports:[
    BsDropdownModule,
    ToastrModule,
    ReactiveFormsModule,
    BsDatepickerModule,
    ModalModule 
    
  ]
})
export class SharedModule { }
