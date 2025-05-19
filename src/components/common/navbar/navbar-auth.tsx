import { Link } from 'react-router'
import { Button } from '../../ui/button'
import { X } from 'lucide-react'

function NavbarAuth() {
  return (
    <nav className="w-full fixed top-0 z-10 border-b backdrop-blur-3xl">
      <div className="px-6 transition-all duration-300">
        <div className="relative flex items-center justify-between gap-6 py-3 lg:gap-0 lg:py-4">
          <Link
            to="/"
            aria-label="home"
            className="text-2xl font-black text-foreground font-sans uppercase"
          >
            PaperNest
          </Link>

          <div className="flex justify-end w-fit gap-3 md:w-fit font-mono tracking-tight text-foreground">
            <Button asChild size="default" variant="outline" className='rounded-full'>
              <Link to="/">
                <X className='size-4'/>
              </Link>
            </Button>
          </div>
        </div>
      </div>
    </nav>
  )
}

export default NavbarAuth
