using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mmx.Models;

namespace mmx.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;
        List<Item> _GradeItems;
        List<Item> _LessonsItems;
        List<Item> _SentenceItems;
        List<Item> _WordItems;


        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级下册", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }

            #region 模拟年级数据
            _GradeItems = new List<Item>();
            var _GradeMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级下册", Description="This is an item description." },
            };

            foreach (var item in _GradeMockItems)
            {
                _GradeItems.Add(item);
            }
            #endregion

            #region 模拟课文数据
            _LessonsItems = new List<Item>();
            var _LessonsMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 1", Description="Welcome back to school!" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 2", Description="My family." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 3", Description="At the zoo!" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 4", Description="Where is my car?" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 5", Description="Do you like pears?" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Unit 6", Description="How many?" },
            };

            foreach (var item in _LessonsMockItems)
            {
                _LessonsItems.Add(item);
            }
            #endregion

            #region 模拟语句数据
            _SentenceItems = new List<Item>();
            var _SentenceMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence1", Description="this is Amy" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence2", Description="Nice to meet you" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence3", Description="Where are you from" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence4", Description="I am from the UK" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence5", Description="Welcome back" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence6", Description="Nice to see you again" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence7", Description="Nice to meet you" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence8", Description="Where are you from" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence9", Description="Nice to see you" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence10", Description="Boys and girls" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence11", Description="we have two new friends today" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence12", Description="I am Mike" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence13", Description="I am from the Canada" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence14", Description="I am Sarah" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sentence15", Description="I am from USA" },
            };

            foreach (var item in _SentenceMockItems)
            {
                _SentenceItems.Add(item);
            }
            #endregion

            #region 模拟单词数据
            _WordItems = new List<Item>();
            var _WordMockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "word1", Description="cat" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "word2", Description="bag" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "word3", Description="hand" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "word4", Description="dad" },
            };

            foreach (var item in _WordMockItems)
            {
                _WordItems.Add(item);
            }
            #endregion
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Item>> GetGradeItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_GradeItems);
        }

        public async Task<IEnumerable<Item>> GetLessonsItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_LessonsItems);
        }

        public async Task<IEnumerable<Item>> GetSentenceItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_SentenceItems);
        }

        public async Task<IEnumerable<Item>> GetWordItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_WordItems);
        }
    }
}