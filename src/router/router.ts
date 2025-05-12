import { createBrowserRouter } from 'react-router'
import MainLayout from '../layout/main-layout'
import Home from '../pages/home/_index'
import ErrorPage from '../pages/error/_index'

const router = createBrowserRouter([
  {
    path: '/',
    Component: MainLayout,
    ErrorBoundary: ErrorPage,
    children: [
      {
        index: true,
        Component: Home,
      },

      {},
    ],
  },
])

export default router
