import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserModel } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';
import {  Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,CommonModule,MatFormFieldModule, MatInputModule, MatButtonModule,MatSelectModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {


    private user = new UserModel();
    public userForm: FormGroup;

    public constructor(
        private userService: UserService,
        private router: Router,
        private formBuilder: FormBuilder, 
        private notifyService: NotifyService
    ) { }


    ngOnInit(): void {
       this.userForm = this.formBuilder.group({
        nameControl: new FormControl("", [Validators.required]),
        emailControl: new FormControl("", [Validators.required, Validators.email]),
        passwordControl: new FormControl("", [Validators.required, Validators.minLength(6)]),
        roleControl: new FormControl("", [Validators.required])
       });
    }

    public async send(){
        try{
            this.user.name = this.userForm.get("nameControl").value;
            this.user.email = this.userForm.get("emailControl").value;
            this.user.password = this.userForm.get("passwordControl").value;
            this.user.roleId = this.userForm.get("roleControl").value;                        
            await this.userService.register(this.user);
            this.notifyService.success(`Welcome ${this.user.name}`);
            this.router.navigateByUrl("/home");
        }
        catch(err : any){
            this.notifyService.error(err);
        }
    }



}
