import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member | undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = []

  constructor(private memberService: MembersService,private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.loadMember();

    this.galleryOptions = [{
      height: '500px',
      width: '500px',
      imageAnimation: NgxGalleryAnimation.Slide,
      // preview: true,
      thumbnailsColumns: 4,
      imagePercent: 100
    }]
  }

  getImages() {
    if(!this.member) return [];

    const imgUrls = [];

    for(const photo of this.member.photos) {
      imgUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      })
    }

    return imgUrls;
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if(username) {
      this.memberService.getMember(username).subscribe({
        next: member => {
          this.member = member;
          this.galleryImages = this.getImages()
        }
      })

    }
  }
}
