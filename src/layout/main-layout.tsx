import { Outlet } from 'react-router'

function MainLayout() {
  return (
    <main className='overflow-hidden text-foreground text-base font-mono'>
      <Outlet/>
    </main>
  )
}

export default MainLayout
