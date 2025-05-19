import { z } from "zod"

const formSchema = z
  .object({
    role: z.enum(['student', 'lecturer'], {
      required_error: 'Please select role first',
    }),
    email: z.string().email({ message: "Email doesn't valid" }),
    name: z.string().min(2, { message: 'Please fill this field' }),
    password: z
      .string()
      .min(8, { message: 'Minimum 8 password character' })
      .regex(/[a-zA-Z]/, {
        message: 'Password must be contains special character (@!-;)',
      })
      .regex(/[0-9]/, {
        message: 'Password must be contains at least one number',
      }),
    confirmPassword: z.string(),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Password doesn't match",
    path: ['confirmPassword'],
  })

export type FormValues = z.infer<typeof formSchema>

export { formSchema}