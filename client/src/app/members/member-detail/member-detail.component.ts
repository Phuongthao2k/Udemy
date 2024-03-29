import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
// import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  standalone: true,
  imports: [CommonModule,TabsModule,GalleryModule]
})
export class MemberDetailComponent implements OnInit {
  member: Member | undefined;
  images: GalleryItem[] = [];

  constructor(private memberService: MembersService,private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.loadMember();
  }

  getImages() {
    if(!this.member) return;

    for(const photo of this.member.photos) {
      this.images.push(new ImageItem({src: photo.url,thumb:photo.url}))

    }
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if(username) {
      this.memberService.getMember(username).subscribe({
        next: member => {
          this.member = member;
          this.getImages();
        }
      })

    }
  }
}
