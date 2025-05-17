import { CheckIcon, XIcon } from 'lucide-react'
import { Button } from '../../../../components/ui/button'
import {
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
} from '../../../../components/ui/form'
import { Input } from '../../../../components/ui/input'
import { PasswordInput } from '../../../../components/ui/password'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { formSchema, type FormValues } from '../../../../helper/sign-in-schema'
import { useEffect, useState } from 'react'
import type { z } from 'zod'
import { useNavigate } from 'react-router'

function SigninForm() {
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

  function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      if (!values.email || !values.password) {
        setError(true)
      }

      const email = localStorage.getItem('paper_nest_email')
      const password = localStorage.getItem('paper_nest_password')

      if (
        email === values.email &&
        (password === values.password || password === values.confirmPassword)
      ) {
        navigate('/workspace')
      }
    } catch (error) {
      console.error('Form submission error', error)
    }
  }
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
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

        <Button variant="default" type="submit" className="w-full">
          Sign in
        </Button>
      </form>
    </Form>
  )
}

export default SigninForm
