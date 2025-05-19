import { Link, useNavigate } from 'react-router'
import NavbarAuth from '../../../components/common/navbar/navbar-auth'
import { Button } from '../../../components/ui/button'
import SignupForm from './components/sign-up-form'
import { useEffect } from 'react'

function SignupPage() {
  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem('paper_nest_token')

    if (token != null) {
      navigate('/dashboard')
    }
  }, [navigate])

  return (
    <>
      <NavbarAuth />
      <section className="-mt-16 sm:mt-0 md:pt-0 flex flex-col items-center min-h-screen relative z-0 bg-background">
        <div
          aria-hidden
          className="absolute -z-10 inset-0 isolate contain-strict"
        >
          <div className="w-140 h-320 -translate-y-87.5 absolute left-0 top-0 -rotate-45 rounded-full bg-[radial-gradient(68.54%_68.72%_at_55.02%_31.46%,hsla(0,0%,85%,.08)_0,hsla(0,0%,55%,.02)_50%,hsla(0,0%,45%,0)_80%)]" />
          <div className="h-420 absolute left-0 top-0 w-60 -rotate-45 rounded-full bg-[radial-gradient(50%_50%_at_50%_50%,hsla(0,0%,85%,.06)_0,hsla(0,0%,45%,.02)_80%,transparent_100%)] [translate:5%_-50%]" />
          <div className="h-320 -translate-y-87.5 absolute left-0 top-0 w-60 -rotate-45 bg-[radial-gradient(50%_50%_at_50%_50%,hsla(0,0%,85%,.04)_0,hsla(0,0%,45%,.02)_80%,transparent_100%)]" />
        </div>
        <div className="max-w-md m-auto h-fit w-full">
          <div className="px-4 md:px-0 space-y-6">
            <div>
              <h1 className="mb-1 mt-4 heading-3">Join Papernest</h1>
              <p className="paragraph">Create account to write your docs</p>
            </div>

            <SignupForm />
          </div>
        </div>
        <p className="mb-6 text-accent-foreground text-center text-sm">
          Have an account?
          <Button asChild variant="link" className="px-2 text-sm">
            <Link to="/signin">Sign in</Link>
          </Button>
        </p>
      </section>
    </>
  )
}

export default SignupPage
