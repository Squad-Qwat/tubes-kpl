import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from './dropdown-menu'
import { Button } from './button'
import { Avatar, AvatarFallback, AvatarImage } from './avatar'
import {
  ChevronUp,
  Command,
  Home,
  LayoutDashboard,
  LogOut,
  Monitor,
  Moon,
  Plus,
  Settings,
  Sun,
} from 'lucide-react'
import { signOutUser } from '../../lib/service/auth-service'
import { useNavigate } from 'react-router'

interface UserAvatarMenuProps {
  name?: string
  email?: string
}

function AvatarDropdown({ name, email }: UserAvatarMenuProps) {
  const navigate = useNavigate()

  const handleLogOut = () => { 
    try {
      signOutUser()
      navigate('/')
    }catch(error) { 
      console.error("Cannot log out", error)
    }
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Avatar className="flex items-center justify-center w-8 h-8 cursor-pointer bg-gradient-to-r from-fuchsia-400 to-red-500">
          <AvatarFallback>{name?.slice(0, 2).toUpperCase()}</AvatarFallback>
        </Avatar>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-fit" align="end" forceMount>
        <DropdownMenuLabel className="font-mono">
          <div className="flex flex-col space-y-1">
            <p className="label">{name}</p>
            <p className="caption">{email}</p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup className="label">
          <DropdownMenuItem className='cursor-pointer'>
            <LayoutDashboard className="w-4 h-4 mr-2" />
            <span>Dashboard</span>
          </DropdownMenuItem>
          <DropdownMenuItem className='cursor-pointer'>
            <Settings className="w-4 h-4 mr-2" />
            <span>Account Settings</span>
          </DropdownMenuItem>
          <DropdownMenuItem className='cursor-pointer'>
            <Plus className="w-4 h-4 mr-2" />
            <span>Create Team</span>
            <div className="ml-auto">
              <Plus className="w-4 h-4" />
            </div>
          </DropdownMenuItem>
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          <DropdownMenuItem className='cursor-pointer'>
            <Command className="w-4 h-4 mr-2" />
            <span>Command Menu</span>
            <div className="flex items-center ml-auto">
              <kbd className="pointer-events-none inline-flex h-5 select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium text-muted-foreground">
                <span>Ctrl</span>
              </kbd>
              <kbd className="pointer-events-none inline-flex h-5 select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium text-muted-foreground ml-1">
                <span>K</span>
              </kbd>
            </div>
          </DropdownMenuItem>
          <DropdownMenuItem className='cursor-pointer'>
            <div className="flex items-center">
              <Monitor className="w-4 h-4 mr-2" />
              <span>Theme</span>
            </div>
            <div className="flex items-center gap-2 ml-auto">
              <Button variant="outline" size="icon" className="w-6 h-6">
                <Monitor className="w-3 h-3" />
              </Button>
              <Button variant="outline" size="icon" className="w-6 h-6">
                <Sun className="w-3 h-3" />
              </Button>
              <Button variant="outline" size="icon" className="w-6 h-6">
                <Moon className="w-3 h-3" />
              </Button>
            </div>
          </DropdownMenuItem>
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuItem className='cursor-pointer'>
          <Home className="w-4 h-4 mr-2" />
          <span>Home Page</span>
          <div className="ml-auto">
            <ChevronUp className="w-4 h-4" />
          </div>
        </DropdownMenuItem>
        <DropdownMenuItem className='cursor-pointer' onClick={handleLogOut}>
          <LogOut className="w-4 h-4 mr-2" />
          <span>Log Out</span>
          <div className="ml-auto">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="16"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
              className="w-4 h-4"
            >
              <path d="M9 6l6 6-6 6" />
              <path d="M5 12h14" />
            </svg>
          </div>
        </DropdownMenuItem>
        <DropdownMenuSeparator />
        <div className="p-2">
          <Button className="w-full">Upgrade to Pro</Button>
        </div>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}

export default AvatarDropdown
