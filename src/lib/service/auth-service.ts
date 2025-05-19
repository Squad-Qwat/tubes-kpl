import { generateRandomNumber } from '../../helper/generator/number'
import AuthLocalStorage from '../../helper/enums/auth'

const API_BASE_URL = import.meta.env.VITE_API_URL
const SIGN_UP_URL: string = API_BASE_URL + "/auth/signup"
const SIGN_IN_URL: string = API_BASE_URL + "/auth/signin"

interface SignUpData {
  name: string
  email: string
  password: string
  role: string
}

interface SignUpResponse {
  success: boolean
  token?: string
  message?: string
}

async function signUpUser(userData: SignUpData): Promise<SignUpResponse> {
  try {
    const response = await fetch(SIGN_UP_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userData),
    })

    if (!response.ok) {
      const errorData = await response.json()
      console.error('API ERROR', response.status, errorData)

      return {
        success: false,
        message: errorData.message || `HTTP ERROR ${response.status}`,
      }
    }

    const data: SignUpResponse = await response.json()

    return {
      ...data,
      success: true,
      token: generateRandomNumber().toString(),
      message: 'Success Sign Up',
    }
  } catch (error) {
    console.error('Network or other error in signUpUser:', error)
    return {
      success: false,
      message: (error as Error).message || 'An unexpected error occurred',
    }
  }
}

async function siginInUser(userData: SignUpData): Promise<SignUpResponse> {
  try {
    const params = new URLSearchParams({
      email: userData.email,
      password: userData.password,
    })

    const url = SIGN_IN_URL + "?" + params.toString()

    const response = await fetch(url, {
      method: 'GET',
    })

    if (!response.ok) {
      const errorData = await response.json()
      console.error('API ERROR', response.status, errorData)

      return {
        success: false,
        message: errorData.message || `HTTP ERROR ${response.status}`,
      }
    }

    const data: SignUpResponse = await response.json()
    return {
      ...data,
      success: true,
      token: generateRandomNumber().toString(),
      message: 'Success Sign Up',
    }
  } catch (error) {
    console.error('Network or other error in signUpUser:', error)
    return {
      success: false,
      message: (error as Error).message || 'An unexpected error occurred',
    }
  }
}

function signOutUser() { 
    try {
        localStorage.removeItem(AuthLocalStorage.status)
        localStorage.removeItem(AuthLocalStorage.token)
        localStorage.removeItem(AuthLocalStorage.user)
    }catch (error) { 
        console.error("Failure to logout", error)
    }
}

export {
    signUpUser,
    siginInUser,
    signOutUser
}