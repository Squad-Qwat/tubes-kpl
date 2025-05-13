import { ArrowLeft } from 'lucide-react'
import { Link } from 'react-router'
import { Button } from '../../../components/ui/button'

function NavbarAuth() {
  return (
    <nav className="bg-background/50 fixed z-20 w-full border-b backdrop-blur-3xl">
      <div className="mx-auto max-w-6xl px-6 transition-all duration-300">
        <div className="relative flex flex-wrap items-center justify-around gap-6 py-3 lg:gap-0 lg:py-4">
          <div className="flex w-full items-center justify-between gap-12 lg:w-auto">
            <Link
              to="/"
              aria-label="home"
              className="flex items-center space-x-2 text-2xl font-black text-foreground font-sans uppercase"
            >
              PaperNest
            </Link>
          </div>

          <Button asChild variant="link" size="default">
            <Link to="/">
              <ArrowLeft/> Back to home
            </Link>
          </Button>
        </div>
      </div>
    </nav>
  )
}

export default NavbarAuth
