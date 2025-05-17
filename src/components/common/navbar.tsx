'use-client'
import { Menu, X } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router'
import { Button } from '../ui/button'
import { navMenuItems } from '../../data/data-navbar'
import { Avatar } from '../ui/avatar'
import { AvatarFallback } from '@radix-ui/react-avatar'

function Navbar() {
  const [menuState, setMenuState] = useState(false)
  const userAuthenticated = localStorage.getItem('paper_nest_name')?.toString()

  const getUserInitials = () :string => { 
    const parts = userAuthenticated?.trim().split(' ').filter(part => part.length > 0)

    if (parts?.length === 0) { 
      return "PN"
    }

    if (parts!.length >= 1) { 
      return (parts![0][0] + parts!.at(-1)![0]).toUpperCase()
    }else {
      return parts![0].substring(0, 2).toUpperCase()
    }
  }  

  return (
    <nav
      data-state={menuState && 'active'}
      className="fixed z-20 w-full border-b bg-background/80 backdrop-blur-3xl paragraph"
    >
      <div className="px-6 mx-auto transition-all duration-300 max-w-7xl lg:px-0">
        <div className="relative flex flex-wrap items-center justify-between gap-6 py-3 lg:gap-0 lg:py-4">
          <div className="flex items-center gap-8">
            <div className="flex items-center w-full gap-12 lg:w-auto">
              <Link
                to="/"
                aria-label="home"
                className="font-sans text-2xl font-black uppercase text-foreground"
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
                      className="block font-mono tracking-tight duration-150 text-muted-foreground hover:text-primary hover:underline"
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
                      className="block duration-150 text-muted-foreground hover:text-primary hover:underline"
                    >
                      <span>{item.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>

            <hr className="h-1" />

            {userAuthenticated != null ? (
              <div className="flex flex-col items-center w-full space-y-3 font-mono tracking-tight sm:flex-row sm:gap-3 sm:space-y-0 md:w-fit text-foreground">
                <Button asChild variant="outline" size="default">
                  <Link to="/auth/signin">
                    <span>Contact</span>
                  </Link>
                </Button>

                <Button asChild variant="outline" size="default">
                  <Link to="/auth/signin">
                    <span>Dashboard</span>
                  </Link>
                </Button>
                
                <Link to="/">
                  <Avatar className='flex items-center justify-center w-8 h-8 bg-gradient-to-r from-fuchsia-400 to-red-500'>
                    <AvatarFallback>{getUserInitials()}</AvatarFallback>
                  </Avatar>
                </Link>
              </div>
            ) : (
              <div className="flex flex-col w-full space-y-3 font-mono tracking-tight sm:flex-row sm:gap-3 sm:space-y-0 lg:w-fit text-foreground">
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
            )}
          </div>
        </div>
      </div>
    </nav>
  )
}

export default Navbar
