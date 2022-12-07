import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/Message';
import { User } from 'src/app/_models/user';
import { PresenceService } from 'src/app/_service/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit,OnDestroy {
 
  @ViewChild('memberTabs' , {static : true}) memberTabs?: TabsetComponent;
  member:Member = {} as Member;
  galeryOptions:NgxGalleryOptions[] = []; 
  galeryImages : NgxGalleryImage[] = [];
  activeTab? : TabDirective;
  messages: Message[] = [];
  user?:User

  constructor(private accountService:AccountService 
              ,private route : ActivatedRoute
              ,private messageService:MessageService
              ,public presenceService:PresenceService) {
                this.accountService.currentUser$.pipe(take(1)).subscribe({
                  next: user => {
                    if(user) this.user=user
                  }
                })
               }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })

    this.galeryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    this.galeryImages = this.getImages();
    
  }
  getImages() {
    if (!this.member) return [];
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }

 

  selectTab(heading : string){
   if (this.memberTabs) {
    this.memberTabs.tabs.find(x => x.heading === heading)!.active = true

   }
    
  }

  loadMessages(){
    if (this.member) {
        this.messageService.getMessageThread(this.member.userName).subscribe({
          next : messages => this.messages = messages
        })
    }
  }

  onTabActivated(data : TabDirective){
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user,this.member.userName);
      
    }else{
      this.messageService.stopHubConnection();
    }
  }

}
