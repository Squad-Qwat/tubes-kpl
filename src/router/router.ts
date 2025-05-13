import { createBrowserRouter } from 'react-router'
import MainLayout from '../layout/main-layout'
import Home from '../pages/home/_index'
import ErrorPage from '../pages/error/_index'
import Signin from '../pages/auth/sign-in'
import Signup from '../pages/auth/sign-up'

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

      {
        path: '/auth/signin',
        Component: Signin
      },

      {
        path: '/auth/signup',
        Component: Signup
      }
    ],
  },
])

export default router
