import { useState, useEffect } from 'react'
import { Button } from '../../../../components/ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../../../../components/ui/form'
import {
  RadioGroup,
  RadioGroupItem,
} from '../../../../components/ui/radio-group'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { formSchema, type FormValues } from '../../../../helper/sign-up-schema'
import { cn } from '../../../../lib/utils'
import { CheckIcon, XIcon } from 'lucide-react'
import { PasswordInput } from '../../../../components/ui/password'
import { Input } from '../../../../components/ui/input'
import type { z } from 'zod'
import { useNavigate } from 'react-router'
import AuthLocalStorage from '../../../../helper/auth'
import { signUpUser } from '../../../../lib/service/auth-service'

const steps = [
  { id: 'role', title: 'Choose role' },
  { id: 'email', title: 'Enter your email' },
  { id: 'name', title: 'Your Fullname' },
  { id: 'password', title: 'Create Password' },
]

function SignupForm() {
  const [currentStep, setCurrentStep] = useState(0)
  const [passwordValue, setPasswordValue] = useState('')
  const [passwordValidation, setPasswordValidation] = useState({
    minLength: false,
    hasLetter: false,
    hasNumber: false,
  })

  const [error, setError] = useState(false)
  const navigate = useNavigate()

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

  useEffect(() => {
    setPasswordValidation({
      minLength: passwordValue.length >= 6,
      hasLetter: /[a-zA-Z]/.test(passwordValue),
      hasNumber: /[0-9]/.test(passwordValue),
    })
  }, [passwordValue])

  const nextStep = async () => {
    const fields = ['role', 'email', 'name', 'password']
    const currentField = fields[currentStep]

    const isValid = await form.trigger(currentField as keyof FormValues)

    if (isValid) {
      setCurrentStep((prev) => Math.min(prev + 1, steps.length - 1))
    }
  }

  const prevStep = () => {
    setCurrentStep((prev) => Math.max(prev - 1, 0))
  }

  async function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      if (!values.role || !values.email || !values.name || !values.password) {
        setError(true)
        localStorage.setItem(AuthLocalStorage.status, String(error))
      } else {
        setError(false)

        const response = await signUpUser({
          name: values.name,
          email: values.email,
          password: values.confirmPassword,
          role: values.role,
        })

        if (response && response.success ) {
          localStorage.setItem(AuthLocalStorage.status, String(error))
          localStorage.setItem(AuthLocalStorage.token, response.token!)

          const user = {
            name: values.name,
            email: values.email
          }
          
          localStorage.setItem(AuthLocalStorage.user, JSON.stringify(user))

          navigate('/workspace')
        } else {
          console.error(
            'Sign up failed: ',
            response ? response.message : 'Unkown error'
          )
        }
      }
    } catch (error) {
      console.error('Form submission error or API error', error)
    }
  }
  return (
    <div className="space-y-6">
      <div className="space-y-2">
        <p className="caption">
          Step {currentStep + 1} - {steps[currentStep].title}
        </p>
        <div className="flex justify-between">
          {steps.map((step, index) => (
            <div
              key={step.id}
              className={cn(
                'w-full h-2 rounded-full mx-1',
                index < currentStep
                  ? 'bg-fuchsia-600'
                  : index === currentStep
                  ? 'bg-fuchsia-400'
                  : 'bg-zinc-700'
              )}
            />
          ))}
        </div>
      </div>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          {currentStep === 0 && (
            <FormField
              control={form.control}
              name="role"
              render={({ field }) => (
                <FormItem className="space-y-3">
                  <FormLabel className="label">Select the role first</FormLabel>
                  <FormControl>
                    <RadioGroup
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                      className="flex flex-col space-y-1"
                    >
                      <FormItem className="flex items-center p-4 space-x-3 space-y-0 border rounded-md border-border">
                        <FormControl>
                          <RadioGroupItem value="student" />
                        </FormControl>
                        <FormLabel className="flex items-center justify-between w-full font-normal cursor-pointer">
                          <span>I'm working on personal projects</span>
                          <span className="px-2 py-1 text-xs text-white rounded bg-accent">
                            Student
                          </span>
                        </FormLabel>
                      </FormItem>
                      <FormItem className="flex items-center p-4 space-x-3 space-y-0 border rounded-md border-border">
                        <FormControl>
                          <RadioGroupItem value="lecturer" />
                        </FormControl>
                        <FormLabel className="flex items-center justify-between w-full font-normal cursor-pointer">
                          <span>I'm working on review the paper</span>
                          <span className="px-2 py-1 text-xs rounded bg-fuchsia-100 text-fuchsia-700">
                            Lecturer
                          </span>
                        </FormLabel>
                      </FormItem>
                    </RadioGroup>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )}

          {currentStep === 1 && (
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
          )}

          {currentStep === 2 && (
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="label">Fullname</FormLabel>
                  <FormControl>
                    <Input placeholder="Masukkan nama lengkap" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )}

          {currentStep === 3 && (
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
                          <CheckIcon className="w-4 h-4 text-green-500" />
                        ) : (
                          <XIcon className="w-4 h-4 text-gray-500" />
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
                          <CheckIcon className="w-4 h-4 text-green-500" />
                        ) : (
                          <XIcon className="w-4 h-4 text-gray-500" />
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
                          <CheckIcon className="w-4 h-4 text-green-500" />
                        ) : (
                          <XIcon className="w-4 h-4 text-gray-500" />
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
                    <FormLabel className="label">Confirm Password</FormLabel>
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
          )}

          <div className="flex justify-between">
            <Button
              variant="secondary"
              onClick={prevStep}
              disabled={currentStep === 0}
            >
              Previous
            </Button>

            {currentStep < steps.length - 1 ? (
              <Button onClick={nextStep}>Continue</Button>
            ) : (
              <Button type="submit" onClick={form.handleSubmit(onSubmit)}>
                Create Account
              </Button>
            )}
          </div>
        </form>
      </Form>
    </div>
  )
}

export default SignupForm
