'use-client'
import { Menu, X } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router'
import { Button } from '../ui/button'
import { navMenuItems } from '../../data/data-navbar'

function Navbar() {
  const [menuState, setMenuState] = useState(false)

  return (
    <nav
      data-state={menuState && 'active'}
      className="fixed z-20 w-full border-b bg-background/80 backdrop-blur-3xl paragraph"
    >
      <div className="mx-auto max-w-7xl px-6 md:px-0 transition-all duration-300">
        <div className="relative flex flex-wrap items-center justify-between gap-6 py-3 lg:gap-0 lg:py-4">
          <div className="flex items-center gap-8">
            <div className="flex w-full items-center gap-12 lg:w-auto">
              <Link
                to="/"
                aria-label="home"
                className="text-2xl font-black text-foreground font-sans uppercase"
              >
                PaperNest
              </Link>
            </div>
            <div className="hidden lg:block">
              <ul className="flex gap-8 text-sm">
                {navMenuItems.map((item, index) => (
                  <li key={index}>
                    <Link
                      to={item.href}
                      className="text-muted-foreground hover:text-primary block hover:underline font-mono tracking-tight duration-150"
                    >
                      <span>{item.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          <button
            onClick={() => setMenuState(!menuState)}
            aria-label={menuState == true ? 'Close Menu' : 'Open Menu'}
            className="relative border rounded-full z-20 -m-2.5 -mr-4 block cursor-pointer p-1.5 lg:hidden"
          >
            <Menu className="in-data-[state=active]:rotate-180 in-data-[state=active]:scale-0 in-data-[state=active]:opacity-0 m-auto size-5 duration-200" />
            <X className="in-data-[state=active]:rotate-0 in-data-[state=active]:scale-100 in-data-[state=active]:opacity-100 absolute inset-0 m-auto size-5 -rotate-180 scale-0 opacity-0 duration-200" />
          </button>

          <div className="in-data-[state=active]:block lg:in-data-[state=active]:flex mb-6 hidden w-full flex-wrap items-center justify-end space-y-4 h-dvh lg:h-max shadow-zinc-300/20 md:flex-nowrap lg:m-0 lg:flex lg:w-fit lg:gap-6 lg:space-y-0 lg:border-transparent lg:bg-transparent lg:p-0 lg:shadow-none dark:shadow-none dark:lg:bg-transparent">
            <div className="lg:hidden">
              <ul className="space-y-6 text-base">
                {navMenuItems.map((item, index) => (
                  <li key={index}>
                    <Link
                      to={item.href}
                      className="text-muted-foreground hover:text-primary block hover:underline duration-150"
                    >
                      <span>{item.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>

            <hr className="h-1" />

            <div className="flex w-full flex-col space-y-3 sm:flex-row sm:gap-3 sm:space-y-0 md:w-fit font-mono tracking-tight text-foreground">
              <Button asChild variant="outline" size="default">
                <Link to="/auth/signin">
                  <span>Contact</span>
                </Link>
              </Button>
              <Button asChild variant="outline" size="default">
                <Link to="/signin">
                  <span>Sign in</span>
                </Link>
              </Button>
              <Button asChild size="default">
                <Link to="/signup">
                  <span>Create Account</span>
                </Link>
              </Button>
            </div>
          </div>
        </div>
      </div>
    </nav>
  )
}

export default Navbar
