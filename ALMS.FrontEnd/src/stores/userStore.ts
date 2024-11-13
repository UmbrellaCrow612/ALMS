import { defineStore } from 'pinia';
import { ref } from 'vue';
import { jwtDecode } from 'jwt-decode';

// Updated interface to match JWT claims structure
interface JWTClaims {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  jti: string;
  exp: number;
  iss: string;
  aud: string;
}

interface User {
  id: string;
  email: string;
  roles: string[];
}

export const useUserStore = defineStore('user', () => {
  const user = ref<User | null>(null);

  const setUser = (token: string) => {
    try {
      const decoded = jwtDecode<JWTClaims>(token);
      console.log("starting decoding");
      console.log(decoded);
      
      // Transform JWT claims into User format
      user.value = {
        id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
        email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        roles: [decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]]
      };
    } catch (error) {
      console.error('Failed to decode token:', error);
      user.value = null;
    }
  };

  const clearUser = () => {
    user.value = null;
    localStorage.removeItem('accessToken');
  };

  return {
    user,
    setUser,
    clearUser,
  };
});