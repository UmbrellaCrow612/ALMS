// SearchView.spec.js
import { mount } from '@vue/test-utils';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import SearchView from '@/views/SearchView.vue';
import MediaDetail from '@/views/MediaDetail.vue';
import axiosInstance from '@/plugins/axios';
import { createRouter, createWebHistory } from 'vue-router';

// Mock axios
vi.mock('@/plugins/axios', () => ({
  default: {
    get: vi.fn(() => Promise.resolve({ data: [] })),
  },
}));

describe('SearchView', () => {
  let router;

  beforeEach(async () => {
    router = createRouter({
      history: createWebHistory(),
      routes: [
        {
          path: '/',
          component: {},
        },
        {
          path: '/media/:id',
          name: 'mediaDetail',
          component: MediaDetail,
          props: true,
          meta: { requiresAuth: true },
        },
      ],
    });
    await router.push('/');
    await router.isReady();
  });

  it('renders the search form with all fields', async () => {
    const wrapper = mount(SearchView, {
      global: {
        plugins: [router],
        stubs: {
          NavBar: true,
        },
      },
    });

    await wrapper.vm.$nextTick();

    expect(wrapper.find('input[placeholder="Search by title"]').exists()).toBe(true);
    expect(wrapper.find('input[placeholder="Author"]').exists()).toBe(true);
    expect(wrapper.find('input[placeholder="Genre"]').exists()).toBe(true);
    expect(wrapper.find('select').exists()).toBe(true);
    expect(wrapper.find('input[type="checkbox"]').exists()).toBe(true);
  });

  it('fetches all media on component mount', async () => {
    const mockData = [{ id: 1, title: 'Test' }];
    axiosInstance.get.mockResolvedValueOnce({ data: mockData });

    const wrapper = mount(SearchView, {
      global: {
        plugins: [router],
        stubs: {
          NavBar: true,
        },
      },
    });

    await wrapper.vm.$nextTick();
    expect(axiosInstance.get).toHaveBeenCalledWith('/media');
  });

  it('handles search submission and filters results', async () => {
    const mockMediaData = [
      { id: 1, title: 'Test Book', author: 'Test Author', mediaType: 1, genre: 'Fiction' },
      { id: 2, title: 'Another Book', author: 'Another Author', mediaType: 2, genre: 'Non-fiction' },
    ];

    axiosInstance.get.mockResolvedValueOnce({ data: mockMediaData });

    const wrapper = mount(SearchView, {
      global: {
        plugins: [router],
        stubs: {
          NavBar: true,
        },
      },
    });

    await wrapper.vm.$nextTick();
    
    await wrapper.find('input[placeholder="Search by title"]').setValue('Test Book');
    await wrapper.find('select').setValue(1);
    await wrapper.find('input[placeholder="Genre"]').setValue('Fiction');
    
    axiosInstance.get.mockResolvedValueOnce({ data: [mockMediaData[0]] });
    
    await wrapper.find('form').trigger('submit.prevent');
    await wrapper.vm.$nextTick();

    expect(axiosInstance.get).toHaveBeenLastCalledWith('/media', {
      params: {
        Title: 'Test Book',
        Author: undefined,
        Genre: 'Fiction',
        MediaType: 1,
        IsAvailable: undefined,
      },
    });
  });

  it('navigates to media detail page on "View Details" click', async () => {
    const mockMediaData = [
      { id: 1, title: 'Test Book', author: 'Test Author', mediaType: 1 },
    ];
    
    axiosInstance.get.mockResolvedValueOnce({ data: mockMediaData });
    
    const routerPushSpy = vi.spyOn(router, 'push').mockResolvedValueOnce();

    const wrapper = mount(SearchView, {
      global: {
        plugins: [router],
        stubs: {
          NavBar: true,
        },
      },
      data() {
        return {
          searchResults: mockMediaData,
        };
      },
    });

    await wrapper.vm.$nextTick();
    await new Promise(resolve => setTimeout(resolve, 0));

    const viewDetailsButton = wrapper.find('button.bg-indigo-500');
    expect(viewDetailsButton.exists()).toBe(true);

    await viewDetailsButton.trigger('click');

    // Check if router.push was called with the correct parameters
    expect(routerPushSpy).toHaveBeenCalledWith({
      name: 'mediaDetail',
      params: { id: 1 }  // Using number instead of string to match the component
    });

    routerPushSpy.mockRestore();
  });
});