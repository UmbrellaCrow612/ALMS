import { mount } from '@vue/test-utils';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import MediaDetail from '@/views/MediaDetail.vue';
import axiosInstance from '@/plugins/axios';
import { createRouter, createWebHistory } from 'vue-router';
import { createPinia, setActivePinia } from 'pinia';

const mockReload = vi.fn();
Object.defineProperty(window, 'location', {
  value: { reload: mockReload }
});

vi.mock('@/plugins/axios', () => ({
  default: {
    get: vi.fn(),
    post: vi.fn(),
  },
}));

describe('MediaDetail', () => {
  let router;
  const mediaId = "4321";
  
  const mockMediaData = {
    id: mediaId,
    title: "Sayings and Anecdotes",
    description: "A book about cynicism",
    publishedAt: "2024-12-14T19:55:46.9674301",
    isbn: "654321",
    imgUrl: "https://picsum.photos/200/300",
    author: "Diogenes",
    genre: "philosophy",
    mediaType: 1,
    isAvailable: true,
    isLost: false,
    dateAdded: "2024-12-14T19:55:46.967427"
  };

  beforeEach(() => {
    setActivePinia(createPinia());
    vi.clearAllMocks();

    router = createRouter({
      history: createWebHistory(),
      routes: [
        { 
          path: '/:id', 
          name: 'media-detail',
          component: MediaDetail 
        }
      ]
    });

    axiosInstance.get.mockImplementation((url) => {
      if (url === `/media/${mediaId}`) {
        return Promise.resolve({ data: mockMediaData });
      }
      if (url === `/media/${mediaId}/history`) {
        return Promise.resolve({ data: [] });
      }
      if (url === '/media') {
        return Promise.resolve({ data: [] });
      }
      return Promise.resolve({ data: [] });
    });
  });

  it('renders media details with all fields', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    expect(wrapper.text()).toContain('Sayings and Anecdotes');
    expect(wrapper.text()).toContain('Diogenes');
    expect(wrapper.text()).toContain('A book about cynicism');
    expect(wrapper.find('img').attributes('src')).toBe('https://picsum.photos/200/300');
    expect(wrapper.text()).toContain('philosophy');
    expect(wrapper.text()).toContain('Book');
    expect(wrapper.text()).toContain(new Date(mockMediaData.publishedAt).toLocaleDateString()); // Use actual formatted date
  });

  it('renders borrow history when available', async () => {
    const mockHistory = ['2024-06-01', '2024-05-15', '2024-04-20'];
    
    // Configure router with the correct base route
    router = createRouter({
      history: createWebHistory(),
      routes: [
        { 
          path: '/media/:id', 
          name: 'media-detail',
          component: MediaDetail 
        }
      ]
    });

    // Mock API responses
    axiosInstance.get.mockImplementation((url) => {
      if (url === `/media/${mediaId}/history`) {
        return Promise.resolve({ data: mockHistory });
      }
      if (url === `/media/${mediaId}`) {
        return Promise.resolve({ data: mockMediaData });
      }
      return Promise.resolve({ data: [] });
    });

    // Push to the media detail route with the correct ID
    await router.push(`/media/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    // Wait for the component to mount and API calls to complete
    await wrapper.vm.$nextTick();
    // Need to wait for both API calls to complete
    await new Promise(resolve => setTimeout(resolve, 0));
    await wrapper.vm.$nextTick();

    // First check if loading state is not visible
    const loadingElement = wrapper.find('[data-test="loading-history"]');
    expect(loadingElement.exists()).toBe(false);

    // Find history items specifically in the borrow history section
    const historyContainer = wrapper.find('[data-test="borrow-history-container"]');
    expect(historyContainer.exists()).toBe(true);

    const historyItems = wrapper.findAll('[data-test="borrow-history-item"]');
    expect(historyItems).toHaveLength(mockHistory.length);

    // Format dates in UK format and verify
    mockHistory.forEach((date, index) => {
      const formattedDate = new Date(date).toLocaleDateString('en-GB', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
      expect(historyItems[index].text()).toContain(formattedDate);
    });
  });

  
  

  // Rest of the test cases...

  it('shows reserve and borrow buttons when media is available', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    expect(wrapper.find('button.bg-emerald-50').exists()).toBe(true);
    expect(wrapper.find('button.bg-blue-600').exists()).toBe(true);
  });

  it('disables reserve and borrow buttons when media is unavailable', async () => {
    const unavailableMedia = { ...mockMediaData, isAvailable: false };
    axiosInstance.get.mockResolvedValueOnce({ data: unavailableMedia });

    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    expect(wrapper.find('button.bg-emerald-50').attributes('disabled')).toBeDefined();
    expect(wrapper.find('button.bg-blue-600').attributes('disabled')).toBeDefined();
  });

  it('shows lost item notification when media is lost', async () => {
    const lostMedia = { ...mockMediaData, isLost: true };
    axiosInstance.get.mockResolvedValueOnce({ data: lostMedia });

    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    expect(wrapper.find('.bg-red-100').exists()).toBe(true);
  });

  it('handles successful borrow and reloads page', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    axiosInstance.post.mockResolvedValueOnce({ status: 200 });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    await wrapper.vm.showBorrowConfirmation();
    expect(wrapper.vm.isBorrowModalVisible).toBe(true);

    await wrapper.vm.confirmBorrow();
    expect(axiosInstance.post).toHaveBeenCalledWith(`/media/${mediaId}/borrow`);
    expect(mockReload).toHaveBeenCalled();
  });

  it('displays error message on failed borrow attempt', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    const errorMessage = 'Borrow limit exceeded';
    axiosInstance.post.mockRejectedValueOnce({ response: { data: errorMessage } });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    await wrapper.vm.showBorrowConfirmation();
    await wrapper.vm.confirmBorrow();

    expect(wrapper.vm.errorMessage).toBe(errorMessage);
  });

  it('handles successful reservation and reloads page', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    axiosInstance.post.mockResolvedValueOnce({ status: 204 });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    await wrapper.vm.reserveItem();
    expect(wrapper.vm.isReserveModalVisible).toBe(true);

    wrapper.vm.reserveFromDate = '2023-06-10';
    wrapper.vm.reserveToDate = '2023-06-15';

    await wrapper.vm.confirmReserve();
    expect(axiosInstance.post).toHaveBeenCalledWith(`/reservations/medias/${mediaId}/reserve`, {
      reserveFrom: new Date('2023-06-10').toISOString(),
      reserveTo: new Date('2023-06-15').toISOString()
    });
    expect(mockReload).toHaveBeenCalled();
  });

  it('displays error message on failed reservation attempt', async () => {
    await router.push(`/${mediaId}`);
    await router.isReady();

    const wrapper = mount(MediaDetail, {
      global: {
        plugins: [router],
        stubs: {
          'NavBar': true,
          'Modal': true,
          'Clock': true,
          'Calendar': true,
          'User': true,
        }
      }
    });

    const errorMessage = 'Reservation failed';
    axiosInstance.post.mockRejectedValueOnce({ response: { data: errorMessage } });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    await wrapper.vm.reserveItem();
    wrapper.vm.reserveFromDate = '2023-06-10';
    wrapper.vm.reserveToDate = '2023-06-15';

    await wrapper.vm.confirmReserve();

    expect(wrapper.vm.reserveError).toBe('Failed to reserve item. Please try again.');
  });
});