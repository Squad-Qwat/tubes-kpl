import { createBrowserRouter } from 'react-router'
import MainLayout from '../layout/main-layout'
import HomePage from '../pages/landing/home/_index'
import ErrorPage from '../pages/error/_index'
import SigninPage from '../pages/landing/auth/sign-in'
import SignupPage from '../pages/landing/auth/sign-up'


const router = createBrowserRouter([
  {
    path: '/',
    Component: MainLayout,
    ErrorBoundary: ErrorPage,
    children: [
      {
        index: true,
        Component: HomePage,
      },

      {
        path: '/',
        children: [
          {
            path: '/auth/signin',
            Component: SigninPage
          },
          {
            path: '/auth/join',
            Component: SignupPage
          }
        ]
      },
    ],
  },
])

export default router
