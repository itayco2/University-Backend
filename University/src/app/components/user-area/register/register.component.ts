import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserModel } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSelectModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    public userForm!: FormGroup;

    public constructor(
        private userService: UserService,
        private router: Router,
        private formBuilder: FormBuilder, 
        private notifyService: NotifyService
    ) {}

    ngOnInit(): void {
       this.userForm = this.formBuilder.group({
            nameControl: this.formBuilder.control("", [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
            emailControl: this.formBuilder.control("", [
                Validators.required,
                Validators.minLength(2),
                Validators.maxLength(100),
                Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')
            ]),
            passwordControl: this.formBuilder.control("", [
                Validators.required,
                Validators.minLength(8),
                Validators.maxLength(100),
                Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$')
            ]),
            roleControl: this.formBuilder.control("", [Validators.required])
       });
    }

    public async send(): Promise<void> {
        if (this.userForm.invalid) return;

        try {
            const newUser: UserModel = {
                name: this.userForm.get("nameControl")?.value,
                email: this.userForm.get("emailControl")?.value,
                password: this.userForm.get("passwordControl")?.value,
                roleId: this.userForm.get("roleControl")?.value
            } as UserModel; // Explicit type casting

            await this.userService.register(newUser);
            this.notifyService.success(`Welcome ${newUser.name}`);
            this.router.navigateByUrl("/home");
        } catch (err: any) {
            this.notifyService.error(err);
        }
    }

    public getFirstErrorMessage(controlName: string): string | null {
        const control = this.userForm.get(controlName);
        if (!control || !control.errors) return null;
    
        const errorMessages: { [key: string]: { [key: string]: string } } = {
            nameControl: {
                required: 'The Name field is required.',
                minlength: "Name can't be less than 2 characters.",
                maxlength: "Name can't exceed 50 characters."
            },
            emailControl: {
                required: 'Email is required.',
                minlength: 'Email must be at least 2 characters long.',
                maxlength: "Email can't exceed 100 characters.",
                pattern: 'Invalid Email format.'
            },
            passwordControl: {
                required: 'Password is required.',
                minlength: 'Password must be at least 8 characters long.',
                maxlength: "Password can't exceed 100 characters.",
                pattern: 'Password must contain at least one uppercase letter, one digit, and one special character.'
            },
            roleControl: {
                required: 'Missing Role.'
            }
        };
    
        const firstErrorKey = Object.keys(control.errors)[0]; 
        return errorMessages[controlName][firstErrorKey] || 'Invalid input.';
    }
    
}
