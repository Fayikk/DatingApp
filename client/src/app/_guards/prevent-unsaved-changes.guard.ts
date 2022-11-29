import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
  //this class is if user while update change itself accout.And user uncorrect,if user click to different area
  //activate this class and user changes saved and doesnt lost.
@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  canDeactivate(component: MemberEditComponent): boolean {
   
   
    if (component.editForm?.dirty) {
      return confirm("Are you sure you want to continue? Any unsaved changes will be lost! ")
    }
    return true
  }
}
