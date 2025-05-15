import NavbarAuth from '../../../components/common/navbar-auth'
import {
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
} from '../../../components/ui/form'
import { Input } from '../../../components/ui/input'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { Button } from '../../../components/ui/button'
import { useState, useEffect } from 'react'
import { CheckIcon, XIcon } from 'lucide-react'
import { PasswordInput } from '../../../components/ui/password'
import { Link } from 'react-router'

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

type FormValues = z.infer<typeof formSchema>

function SigninPage() {
  const [passwordValue, setPasswordValue] = useState('')
  const [passwordValidation, setPasswordValidation] = useState({
    minLength: false,
    hasLetter: false,
    hasNumber: false,
  })

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      role: 'student',
      email: '',
      name: '',
      password: '',
      confirmPassword: '',
    },
    mode: 'onChange',
  })

  // Memantau perubahan pada password untuk validasi real-time
  useEffect(() => {
    setPasswordValidation({
      minLength: passwordValue.length >= 6,
      hasLetter: /[a-zA-Z]/.test(passwordValue),
      hasNumber: /[0-9]/.test(passwordValue),
    })
  }, [passwordValue])

  function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      console.log(values)
    } catch (error) {
      console.error('Form submission error', error)
    }
  }

  return (
    <>
      <NavbarAuth />
      <section className="pt-20 md:pt-0 md:flex flex-col items-center min-h-screen relative z-0 bg-background">
        <div
          aria-hidden
          className="absolute -z-10 inset-0 isolate contain-strict"
        >
          <div className="w-140 h-320 -translate-y-87.5 absolute left-0 top-0 -rotate-45 rounded-full bg-[radial-gradient(68.54%_68.72%_at_55.02%_31.46%,hsla(0,0%,85%,.08)_0,hsla(0,0%,55%,.02)_50%,hsla(0,0%,45%,0)_80%)]" />
          <div className="h-420 absolute left-0 top-0 w-60 -rotate-45 rounded-full bg-[radial-gradient(50%_50%_at_50%_50%,hsla(0,0%,85%,.06)_0,hsla(0,0%,45%,.02)_80%,transparent_100%)] [translate:5%_-50%]" />
          <div className="h-320 -translate-y-87.5 absolute left-0 top-0 w-60 -rotate-45 bg-[radial-gradient(50%_50%_at_50%_50%,hsla(0,0%,85%,.04)_0,hsla(0,0%,45%,.02)_80%,transparent_100%)]" />
        </div>
        <div className="max-w-md m-auto h-fit w-full">
          <div className="p-6 rounded-2xl space-y-6">
            <div>
              <h1 className="mb-1 mt-4 heading-3">Sign in</h1>
              <p className="paragraph">Welcome back to Papernest!</p>
            </div>

            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(onSubmit)}
                className="space-y-4"
              >
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <>
                      <FormItem>
                        <FormLabel className="label">Email</FormLabel>
                        <FormControl>
                          <Input placeholder="email@example.com" {...field} />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    </>
                  )}
                />

                <div className="space-y-4">
                  <FormField
                    control={form.control}
                    name="password"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="label">Password</FormLabel>
                        <FormControl>
                          <PasswordInput
                            placeholder="Create a password"
                            {...field}
                            onChange={(e) => {
                              field.onChange(e)
                              setPasswordValue(e.target.value)
                            }}
                          />
                        </FormControl>
                        <div className="mt-2 space-y-2">
                          <div className="flex items-center space-x-2 text-sm">
                            {passwordValidation.minLength ? (
                              <CheckIcon className="h-4 w-4 text-green-500" />
                            ) : (
                              <XIcon className="h-4 w-4 text-gray-500" />
                            )}
                            <span
                              className={
                                passwordValidation.minLength
                                  ? 'text-green-500'
                                  : 'text-gray-500'
                              }
                            >
                              Minumum 6 character
                            </span>
                          </div>
                          <div className="flex items-center space-x-2 text-sm">
                            {passwordValidation.hasLetter ? (
                              <CheckIcon className="h-4 w-4 text-green-500" />
                            ) : (
                              <XIcon className="h-4 w-4 text-gray-500" />
                            )}
                            <span
                              className={
                                passwordValidation.hasLetter
                                  ? 'text-green-500'
                                  : 'text-gray-500'
                              }
                            >
                              Contains special character
                            </span>
                          </div>
                          <div className="flex items-center space-x-2 text-sm">
                            {passwordValidation.hasNumber ? (
                              <CheckIcon className="h-4 w-4 text-green-500" />
                            ) : (
                              <XIcon className="h-4 w-4 text-gray-500" />
                            )}
                            <span
                              className={
                                passwordValidation.hasNumber
                                  ? 'text-green-500'
                                  : 'text-gray-500'
                              }
                            >
                              Contains at least one number
                            </span>
                          </div>
                        </div>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="confirmPassword"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="label">
                          Confirm Password
                        </FormLabel>
                        <FormControl>
                          <PasswordInput
                            placeholder="Confirm your password"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                <Button variant="default" type="submit" className="w-full">
                  Sign in
                </Button>
              </form>
            </Form>

            <div className="grid grid-cols-[1fr_auto_1fr] items-center gap-3">
              <hr className="border-dashed" />
              <span className="text-muted-foreground uppercase text-xs">
                Or continue With
              </span>
              <hr className="border-dashed" />
            </div>

            <div>
              <Button type="button" variant="secondary" className="w-full">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="0.98em"
                  height="1em"
                  viewBox="0 0 256 262"
                >
                  <path
                    fill="#4285f4"
                    d="M255.878 133.451c0-10.734-.871-18.567-2.756-26.69H130.55v48.448h71.947c-1.45 12.04-9.283 30.172-26.69 42.356l-.244 1.622l38.755 30.023l2.685.268c24.659-22.774 38.875-56.282 38.875-96.027"
                  ></path>
                  <path
                    fill="#34a853"
                    d="M130.55 261.1c35.248 0 64.839-11.605 86.453-31.622l-41.196-31.913c-11.024 7.688-25.82 13.055-45.257 13.055c-34.523 0-63.824-22.773-74.269-54.25l-1.531.13l-40.298 31.187l-.527 1.465C35.393 231.798 79.49 261.1 130.55 261.1"
                  ></path>
                  <path
                    fill="#fbbc05"
                    d="M56.281 156.37c-2.756-8.123-4.351-16.827-4.351-25.82c0-8.994 1.595-17.697 4.206-25.82l-.073-1.73L15.26 71.312l-1.335.635C5.077 89.644 0 109.517 0 130.55s5.077 40.905 13.925 58.602z"
                  ></path>
                  <path
                    fill="#eb4335"
                    d="M130.55 50.479c24.514 0 41.05 10.589 50.479 19.438l36.844-35.974C195.245 12.91 165.798 0 130.55 0C79.49 0 35.393 29.301 13.925 71.947l42.211 32.783c10.59-31.477 39.891-54.251 74.414-54.251"
                  ></path>
                </svg>
                <span>Google</span>
              </Button>
            </div>
          </div>
        </div>
        <p className="mb-6 text-accent-foreground text-center text-sm">
          New to PaperNest?
          <Button asChild variant="link" className="px-2 text-sm">
            <Link to="/auth/join">Create Account</Link>
          </Button>
        </p>
      </section>
    </>
  )
}

export default SigninPage
