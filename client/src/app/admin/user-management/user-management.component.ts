import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users:User[] = []
  bsModalRef:BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>(); 

  constructor(private adminService:AdminService,private modalService:BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles()
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next : users => this.users = users
    })
  }

  
}
